﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Generator.Merge
{
    public class DirectoryMerge : IDirectoryMerge
    {
        private readonly IFileMerge _merge;

        public DirectoryMerge(IFileMerge merge)
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

        public async Task MergeDirectoryAsync(string originalDirectoryPath, string theirDirectoryPath, string myDirectoryPath, IEnumerable<(string Original, string Their)> renames = null)
        {
            var renameList = renames != null ? renames.ToList() : new List<(string, string)>();
            var compareFiles = DirectoryCompareHelper.Compare(originalDirectoryPath, theirDirectoryPath, renameList);
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
                    else
                    {
                        myFilePath = Path.Combine(myDirectoryPath, compareFile.Name);
                        if (File.Exists(myFilePath))
                        {
                            await MergeFileAsync(compareFile.LeftPath, compareFile.RightPath, myFilePath);
                        }
                    }
                }
            }
        }
    }
}
