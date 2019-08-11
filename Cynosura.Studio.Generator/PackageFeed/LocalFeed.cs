using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Microsoft.Extensions.Options;

namespace Cynosura.Studio.Generator.PackageFeed
{
    public class LocalFeed : IPackageFeed
    {
        private readonly LocalFeedOptions _options;
        private const string Extension = ".nupkg";
        private const string ZeroVersion = "0.0.0";

        public LocalFeed(IOptions<LocalFeedOptions> options)
        {
            _options = options.Value;
        }

        public Task<IList<string>> GetVersionsAsync(string packageName)
        {
            var result = Directory.GetFiles(_options.SourcePath)
                .Select(s => new FileInfo(s))
                .Where(w => w.Name.EndsWith(Extension))
                .Where(w => w.Name.StartsWith(packageName))
                .Select(s => GetVersion(packageName, s.FullName))
                .Where(s => s != null)
                .ToList() as IList<string>;
            return Task.FromResult(result);
        }

        private string GetVersion(string packageName, string path)
        {
            using (var file = File.OpenRead(path))
            using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
            {
                var nuspec = zip.GetEntry($"{packageName}.nuspec");
                using (var info = nuspec.Open())
                using (var reader = new StreamReader(info, Encoding.UTF8))
                {
                    var xml = reader.ReadToEnd();
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    switch (xmlDoc.DocumentElement.NamespaceURI)
                    {
                        case "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd":
                            var package201007 = xml.DeserializeDataContract<NugetPackage201007>();
                            if (package201007.Metadata.Id != packageName)
                            {
                                return null;
                            }
                            return package201007.Metadata.Version;
                        case "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd":
                            var package201305 = xml.DeserializeDataContract<NugetPackage201305>();
                            if (package201305.Metadata.Id != packageName)
                            {
                                return null;
                            }
                            return package201305.Metadata.Version;
                        default:
                            return null;
                    }
                }
            }
        }

        public async Task<string> DownloadPackageAsync(string path, string packageName, string version)
        {
            var fileName = version == ZeroVersion ? $"{packageName}{Extension}" : $"{packageName}.{version}{Extension}";
            var targetPath = Path.Combine(_options.SourcePath, fileName);

            if (!File.Exists(targetPath))
            {
                var item = Directory.GetFiles(_options.SourcePath)
                    .Select(s => new FileInfo(s))
                    .Where(w => w.Name.EndsWith(Extension))
                    .Where(w => w.Name.StartsWith(packageName))
                    .Select(s => new
                    {
                        Path = s.FullName,
                        Version = GetVersion(packageName, s.FullName)
                    })
                    .FirstOrDefault(s => s.Version == version);
                if (item == null)
                {
                    throw new FileNotFoundException();
                }

                targetPath = item.Path;
            }

            var filePath = Path.Combine(path, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var source = File.OpenRead(targetPath))
            using (var streamWriter = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await source.CopyToAsync(streamWriter);
            }

            var extractedPath = Path.Combine(path, $"{packageName}.{version}");
            if (Directory.Exists(extractedPath))
            {
                Directory.Delete(extractedPath, true);
            }
            ZipFile.ExtractToDirectory(filePath, extractedPath);
            var systemPath = Path.Combine(extractedPath, "content", ".template.config");
            if (Directory.Exists(systemPath))
                Directory.Delete(systemPath, true);

            return Path.Combine(extractedPath, "content");
        }
    }
}
