using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.PackageFeed
{
    public interface IPackageFeed
    {
        Task<IList<string>> GetVersionsAsync(string packageName);
        Task<string> DownloadPackageAsync(string path, string packageName, string version);
    }
}
