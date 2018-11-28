using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.PackageFeed
{
    public class LocalFeed : IPackageFeed
    {
        public Task<IList<string>> GetVersionsAsync(string packageName) => Task.FromResult<IList<string>>(new List<string>(){"0.0.0"});

        public async Task<string> DownloadPackageAsync(string path, string packageName, string version)
        {
            var fileName = $"{packageName}.nupkg";
            var target = new [] {Directory.GetCurrentDirectory(),"..", "..", "Cynosura.Template", "template", "artifacts", fileName };
            var targetPath = Path.Combine(target);

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
            if (!Directory.Exists(extractedPath))
            {
                ZipFile.ExtractToDirectory(filePath, extractedPath);
                var systemPath = Path.Combine(extractedPath, "content", ".template.config");
                if (Directory.Exists(systemPath))
                    Directory.Delete(systemPath, true);
            }
            return Path.Combine(extractedPath, "content");
        }
    }
}