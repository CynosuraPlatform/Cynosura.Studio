using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Merge;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Cynosura.Studio.Core.Tests
{
    [TestFixture]
    public class FileMergeTests
    {
        [Test]
        public async Task MergeDirectoryAsync_Simple()
        {
            var dir1 = InitDirectory(new[] {new FileInfo("file.txt", "abd def ghi")});
            var dir2 = InitDirectory(new[] {new FileInfo("file.txt", "abd def tyu ghi")});
            var dir3 = InitDirectory(new[] { new FileInfo("file.txt", "abd plk def ghi") });
            var fileMerge = new FileMerge(new DmpMerge());
            await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
            var files = ReadDirectory(dir3).ToList();
            Assert.That(files.Count, Is.EqualTo(1));
            Assert.That(files[0].Content, Is.EqualTo("abd plk def tyu ghi"));
            Directory.Delete(dir1, true);
            Directory.Delete(dir2, true);
            Directory.Delete(dir3, true);
        }

        private string InitDirectory(IEnumerable<FileInfo> files)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "Cynosura.Studio.Core.Tests", Guid.NewGuid().ToString());
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            foreach (var file in files)
            {
                File.WriteAllText(Path.Combine(tempPath, file.Path), file.Content);
            }
            return tempPath;
        }

        private IEnumerable<FileInfo> ReadDirectory(string path)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                yield return new FileInfo(file, File.ReadAllText(file));
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
