using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Merge
{
    public class FileMerge
    {
        private readonly IMerge _merge;

        public FileMerge(IMerge merge)
        {
            _merge = merge;
        }

        private async Task<string> ReadFileAsync(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return await fileReader.ReadToEndAsync();
            }
        }

        private async Task WriteFileAsync(string filePath, string content)
        {
            using (var fileWriter = new StreamWriter(filePath))
            {
                await fileWriter.WriteAsync(content);
            }
        }

        private void EnsureDirectoryExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public async Task MergeFileAsync(string originalFilePath, string theirFilePath, string myFilePath)
        {
            var original = await ReadFileAsync(originalFilePath);
            var their = await ReadFileAsync(theirFilePath);
            if (original == their)
                return;
            var my = await ReadFileAsync(myFilePath);
            var resultText = _merge.Merge(original, their, my);
            if (resultText == my)
                return;
            await WriteFileAsync(myFilePath, resultText);
        }

        private IList<string> GetFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        }

        private string GetPathAfterRename(string path, IList<(string Left, string Right)> renames)
        {
            foreach (var (left, right) in renames)
            {
                if (path == left)
                    return right;
            }
            return path;
        }

        private IList<FileCompare> Compare(string leftPath, string rightPath, IList<(string Left, string Right)> renames)
        {
            var leftFiles = GetFiles(leftPath)
                .Select(f => new FileCompare()
                {
                    OriginalName = Path.GetRelativePath(leftPath, f),
                    Name = GetPathAfterRename(Path.GetRelativePath(leftPath, f), renames),
                    LeftPath = f,
                });
            var rightFiles = GetFiles(rightPath)
                .Select(f => new FileCompare()
                {
                    Name = Path.GetRelativePath(rightPath, f),
                    RightPath = f,
                });
            return leftFiles.Concat(rightFiles)
                .GroupBy(f => f.Name)
                .Select(g => new FileCompare()
                {
                    Name = g.Key,
                    OriginalName = g.Select(i => i.OriginalName).FirstOrDefault(i => i != null) ?? g.Key,
                    LeftPath = g.Select(i => i.LeftPath).FirstOrDefault(i => i != null),
                    RightPath = g.Select(i => i.RightPath).FirstOrDefault(i => i != null),
                })
                .ToList();
        }

        public async Task MergeDirectoryAsync(string originalDirectoryPath, string theirDirectoryPath, string myDirectoryPath, IEnumerable<(string Original, string Their)> renames = null)
        {
            var renameList = renames != null ? renames.ToList() : new List<(string, string)>();
            var compareFiles = Compare(originalDirectoryPath, theirDirectoryPath, renameList);
            foreach (var compareFile in compareFiles)
            {
                var myFilePath = Path.Combine(myDirectoryPath, compareFile.OriginalName);
                if (compareFile.LeftPath == null)
                {
                    var rightFileContent = await ReadFileAsync(compareFile.RightPath);
                    EnsureDirectoryExists(myFilePath);
                    await WriteFileAsync(myFilePath, rightFileContent);
                }
                else if (compareFile.RightPath == null)
                {
                    if (File.Exists(myFilePath))
                        File.Delete(myFilePath);
                }
                else
                {
                    if (File.Exists(myFilePath))
                    {
                        if (compareFile.Name != compareFile.OriginalName)
                        {
                            var newMyFilePath = Path.Combine(myDirectoryPath, compareFile.Name);
                            EnsureDirectoryExists(newMyFilePath);
                            File.Move(myFilePath, newMyFilePath);
                            myFilePath = newMyFilePath;
                        }
                        await MergeFileAsync(compareFile.LeftPath, compareFile.RightPath, myFilePath);
                    }
                }
            }
        }

        class FileCompare
        {
            public string OriginalName { get; set; }
            public string Name { get; set; }
            public string LeftPath { get; set; }
            public string RightPath { get; set; }
        }
    }
}
