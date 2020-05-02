using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cynosura.Studio.Generator.Tests
{
    class FileHelper
    {
        public static string InitDirectory(IEnumerable<FileInfo> files)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "Cynosura.Studio.Core.Tests", Guid.NewGuid().ToString());
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            foreach (var file in files)
            {
                var filePath = Path.Combine(tempPath, file.Path);
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, file.Content);
            }
            return tempPath;
        }

        public static IEnumerable<FileInfo> ReadDirectory(string path)
        {
            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(path, file);
                yield return new FileInfo(relativePath, File.ReadAllText(file));
            }
        }

        public class FileInfo
        {
            public string Path { get; set; }
            public string Content { get; set; }

            public FileInfo(string path, string content)
            {
                Path = path;
                Content = content;
            }
        }
    }
}
