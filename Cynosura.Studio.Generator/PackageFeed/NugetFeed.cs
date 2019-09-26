using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cynosura.Core.Services;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Cynosura.Studio.Generator.PackageFeed
{
    public class NugetFeed : IPackageFeed
    {
        private readonly NugetSettings _settings;

        public NugetFeed(IOptions<NugetSettings> settings)
        {
            _settings = settings.Value;
        }

        private HttpClient GetHttpClient()
        {
            if (string.IsNullOrEmpty(_settings.FeedUrl))
            {
                throw new ServiceException("NugetFeed/FeedUrl not configured");
            }
            var httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(_settings.Username) || !string.IsNullOrEmpty(_settings.Password))
            {
                var encryptedCredentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encryptedCredentials);
            }
            return httpClient;
        }

        private async Task<FeedData> GetFeedDataAsync()
        {
            var httpClient = GetHttpClient();
            var feedResult = await httpClient.GetStringAsync(_settings.FeedUrl);
            var feed = feedResult.DeserializeFromJson<FeedData>();
            return feed;
        }

        private async Task<string> GetPackageBaseAddressAsync()
        {
            var feed = await GetFeedDataAsync();
            return feed.GetExplicitService("PackageBaseAddress/3.0.0");
        }

        private async Task<string> GetSearchAutocompleteServiceAsync()
        {
            var feed = await GetFeedDataAsync();
            return feed.GetExplicitService("SearchAutocompleteService");
        }
        
        private async Task<string> GetRegistrationsBaseUrlAsync()
        {
            var feed = await GetFeedDataAsync();
            return feed.GetType("RegistrationsBaseUrl", _settings.ListingApiVersion ??  "3.6.0");
        }

        public async Task<IList<string>> GetVersionsAsync(string packageName)
        {
            var httpClient = GetHttpClient();
            if (_settings.ListingApi == NugetListingApi.RegistrationsBaseUrl)
            {
                var registrationsBase = await GetRegistrationsBaseUrlAsync();
                var versionsUrl = $"{registrationsBase}/{packageName.ToLower()}";
                var versionsResult = await httpClient.GetStringAsync(versionsUrl);
                var response = versionsResult.DeserializeFromJson<RegistrationResponse>();
                var versions = response.Items.SelectMany(s => s.Items)
                    .Where(w => w.CatalogEntry.Listed)
                    .Select(s => s.CatalogEntry.Version);
                return OrderVersionsDescending(versions);
            }
            else
            {
                var searchAutocompleteService = await GetSearchAutocompleteServiceAsync();
                var versionsUrl = $"{searchAutocompleteService}?id={packageName.ToLower()}&prerelease=true";
                var versionsResult = await httpClient.GetStringAsync(versionsUrl);
                var versions = versionsResult.DeserializeFromJson<VersionData>();
                return OrderVersionsDescending(versions.Data);
            }
        }

        public async Task<string> DownloadPackageAsync(string path, string packageName, string version)
        {
            var fileName = $"{packageName}.{version}.nupkg";
            var filePath = Path.Combine(path, fileName);
            if (!File.Exists(filePath))
            {
                var baseAddress = await GetPackageBaseAddressAsync();
                var packageUrl = $"{baseAddress}/{packageName.ToLower()}/{version.ToLower()}/{packageName.ToLower()}.{version.ToLower()}.nupkg";
                var httpClient = GetHttpClient();
                using (var resultStream = await httpClient.GetStreamAsync(packageUrl))
                {
                    using (var streamWriter = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await resultStream.CopyToAsync(streamWriter);
                    }
                }
            }
            var extractedPath = Path.Combine(path, $"{packageName}.{version}");
            if (Directory.Exists(extractedPath))
                Directory.Delete(extractedPath, true);
            Extract(filePath, extractedPath);
            var systemPath = Path.Combine(extractedPath, "content", ".template.config");
            if (Directory.Exists(systemPath))
                Directory.Delete(systemPath, true);
            return Path.Combine(extractedPath, "content");
        }

        private IList<string> OrderVersionsDescending(IEnumerable<string> versions)
        {
            return versions.OrderByDescending(v => v, new VersionComparer()).ToList();
        }

        private void Extract(string sourcePath, string destinationPath)
        {
            ZipFile.ExtractToDirectory(sourcePath, destinationPath);
        }

        public class FeedData
        {
            public string Version { get; set; }
            public IList<FeedResource> Resources { get; set; }

            public string GetExplicitService(string explicitType)
            {
                return Resources.Where(w => w.Type == explicitType)
                    .Select(s => s.Id)
                    .FirstOrDefault();
            }

            public IEnumerable<string> GetTypes(string type)
            {
                return Resources.Where(w => w.Type == type || w.Type.StartsWith(type + "/"))
                    .Select(s => s.Type);
            }

            public string GetType(string type, string startVersion)
            {
                var types = GetTypes(type);
                var versions = new List<KeyValuePair<string, NugetVersion>>();
                foreach (var resourceType in types)
                {
                    if (resourceType == $"{type}/Versioned")
                        versions.Add(new KeyValuePair<string, NugetVersion>(
                            Resources.First(f => f.Type == resourceType).Id, new NugetVersion($"999.999")));
                    if (resourceType.StartsWith(type + "/"))
                    {
                        var inner = resourceType.Replace(type + "/", "");
                        if (NugetVersion.IsValid(inner))
                        {
                            versions.Add(new KeyValuePair<string, NugetVersion>(
                                Resources.First(f => f.Type == resourceType).Id, new NugetVersion(inner)));
                        }
                    }
                }
                
                var target = new NugetVersion(startVersion);
                return versions
                    .Where(w => w.Value.Version >= target.Version)
                    .OrderByDescending(d => d.Value.Version)
                    .Select(s => s.Key)
                    .FirstOrDefault();
            }
        }
    }
}

