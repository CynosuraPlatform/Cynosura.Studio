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
using Cynosura.Studio.Core.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.PackageFeed
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
                throw new StudioException("NugetFeed/FeedUrl not configured", "NugetFeed/FeedUrl");
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

        private async Task<string> GetPackageBaseAddressAsync()
        {
            var httpClient = GetHttpClient();
            var feedResult = await httpClient.GetStringAsync(_settings.FeedUrl);
            var feed = feedResult.DeserializeFromJson<FeedData>();
            var baseAddress = feed.Resources.Where(r => r.Type == "PackageBaseAddress/3.0.0")
                .Select(r => r.Id)
                .FirstOrDefault();
            if (baseAddress != null)
                baseAddress = Regex.Replace(baseAddress, "/$", "");
            return baseAddress;
        }

        public async Task<IList<string>> GetVersionsAsync(string packageName)
        {
            var baseAddress = await GetPackageBaseAddressAsync();
            var versionsUrl = $"{baseAddress}/{packageName.ToLower()}/index.json";
            var httpClient = GetHttpClient();
            var versionsResult = await httpClient.GetStringAsync(versionsUrl);
            var versions = versionsResult.DeserializeFromJson<VersionData>();
            return OrderVersionsDescending(versions.Versions);
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

        private IList<string> OrderVersionsDescending(IList<string> versions)
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
        }

        public class FeedResource
        {
            [JsonProperty(PropertyName = "@id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "@type")]
            public string Type { get; set; }

            public string Comment { get; set; }
        }

        public class VersionData
        {
            public IList<string> Versions { get; set; }
        }

        class VersionComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                var xSplit = x.Split('.', '-');
                var ySplit = y.Split('.', '-');
                for (int i = 0; i < Math.Min(xSplit.Length, ySplit.Length); i++)
                {
                    int compare;
                    if (int.TryParse(xSplit[i], out var xNumber) && int.TryParse(ySplit[i], out var yNumber))
                    {
                        compare = xNumber.CompareTo(yNumber);
                    }
                    else
                    {
                        compare = xSplit[i].CompareTo(ySplit[i]);
                    }
                    if (compare != 0)
                        return compare;
                }
                return 0;
            }
        }
    }
}
