using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cynosura.Studio.Generator.Merge
{
    public class DirectoryCompareHelper
    {
        private static bool FileContains(string fileName, IList<string> ignores)
        {
            if (ignores == null)
                return false;
            if (ignores.Any(ignore => fileName.StartsWith(ignore)))
                return true;
            return false;
        }

        private static IList<string> GetFiles(string directoryPath, IList<string> ignores = null)
        {
            return Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories)
                .Where(f => !FileContains(Path.GetRelativePath(directoryPath, f), ignores))
                .ToList();
        }

        private static string GetPathAfterRename(string path, IList<(string Left, string Right)> renames)
        {
            if (renames != null)
            {
                foreach (var (left, right) in renames)
                {
                    if (path.StartsWith(left))
                        return right + path.Substring(left.Length);
                }
            }
            return path;
        }

        public static IList<FileCompare> Compare(string leftPath, string rightPath, IList<(string Left, string Right)> renames = null, IList<string> ignores = null)
        {
            var leftFiles = GetFiles(leftPath, ignores)
                .Select(f => new FileCompare()
                {
                    OriginalName = Path.GetRelativePath(leftPath, f),
                    Name = GetPathAfterRename(Path.GetRelativePath(leftPath, f), renames),
                    LeftPath = f,
                });
            var rightFiles = GetFiles(rightPath, ignores)
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
    }

    public class FileCompare
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string LeftPath { get; set; }
        public string RightPath { get; set; }
    }
}
