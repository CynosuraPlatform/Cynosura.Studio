using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Generator.Merge
{
    public interface IDirectoryMerge
    {
        Task MergeDirectoryAsync(string originalDirectoryPath, string theirDirectoryPath, string myDirectoryPath, IEnumerable<(string Original, string Their)> renames = null);
    }
}
