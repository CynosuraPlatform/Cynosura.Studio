using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Merge
{
    public interface IMerge
    {
        Task MergeFileAsync(string originalFilePath, string theirFilePath, string myFilePath);
        Task MergeDirectoryAsync(string originalDirectoryPath, string theirDirectoryPath, string myDirectoryPath);
    }
}
