using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
            var httpClient = new HttpClient();
            var encryptedCredentials = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encryptedCredentials);
            return httpClient;
        }

        private async Task<string> GetPackageBaseAddressAsync()
        {
            var httpClient = GetHttpClient();
            var feedResult = await httpClient.GetStringAsync(_settings.FeedUrl);
            var feed = JsonConvert.DeserializeObject<FeedData>(feedResult);
            return feed.Resources.Where(r => r.Type == "PackageBaseAddress/3.0.0")
                .Select(r => r.Id)
                .FirstOrDefault();
        }

        public async Task<IList<string>> GetVersionsAsync(string packageName)
        {
            var baseAddress = await GetPackageBaseAddressAsync();
            var versionsUrl = $"{baseAddress}/{packageName.ToLower()}/index.json";
            var httpClient = GetHttpClient();
            var versionsResult = await httpClient.GetStringAsync(versionsUrl);
            var versions = JsonConvert.DeserializeObject<VersionData>(versionsResult);
            return versions.Versions;
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
            if (!Directory.Exists(extractedPath))
            {
                Extract(filePath, extractedPath);
                var systemPath = Path.Combine(extractedPath, "content", ".template.config");
                if (Directory.Exists(systemPath))
                    Directory.Delete(systemPath, true);
            }
            return Path.Combine(extractedPath, "content");
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
    }
}
