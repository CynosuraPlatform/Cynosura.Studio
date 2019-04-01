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
        public async Task MergeDirectoryAsync_SimpleMerge()
        {
            var dir1 = InitDirectory(new[] { new FileInfo("file.txt", "abd def ghi") });
            var dir2 = InitDirectory(new[] { new FileInfo("file.txt", "abd def tyu ghi") });
            var dir3 = InitDirectory(new[] { new FileInfo("file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new FileMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file.txt"));
                Assert.That(files[0].Content, Is.EqualTo("abd plk def tyu ghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_Create()
        {
            var dir1 = InitDirectory(new FileInfo[] { });
            var dir2 = InitDirectory(new[] { new FileInfo("file.txt", "abd def tyu ghi") });
            var dir3 = InitDirectory(new FileInfo[] { });
            try
            {
                var fileMerge = new FileMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file.txt"));
                Assert.That(files[0].Content, Is.EqualTo("abd def tyu ghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_Delete()
        {
            var dir1 = InitDirectory(new[] { new FileInfo("file.txt", "abd def ghi") });
            var dir2 = InitDirectory(new FileInfo[] { });
            var dir3 = InitDirectory(new[] { new FileInfo("file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new FileMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(0));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test] public async Task MergeDirectoryAsync_MergeDeleted()
        {
            var dir1 = InitDirectory(new[] { new FileInfo("file.txt", "abd def ghi") });
            var dir2 = InitDirectory(new[] { new FileInfo("file.txt", "abd def tyu ghi") });
            var dir3 = InitDirectory(new FileInfo[] { });
            try
            {
                var fileMerge = new FileMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(0));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_MergeWithRename()
        {
            var dir1 = InitDirectory(new[] { new FileInfo("file.txt", "abd def ghi") });
            var dir2 = InitDirectory(new[] { new FileInfo("file2.txt", "abd def tyu ghi") });
            var dir3 = InitDirectory(new[] { new FileInfo("file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new FileMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3,
                    new[] { ("file.txt", "file2.txt") });
                var files = ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file2.txt"));
                Assert.That(files[0].Content, Is.EqualTo("abd plk def tyu ghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_MergeWithRenameDirectory()
        {
            var dir1 = InitDirectory(new[] { new FileInfo("path1/file.txt", "abd def ghi") });
            var dir2 = InitDirectory(new[] { new FileInfo("path2/file.txt", "abd def tyu ghi") });
            var dir3 = InitDirectory(new[] { new FileInfo("path1/file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new FileMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3,
                    new[] { ("path1", "path2") });
                var files = ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("path2" + Path.DirectorySeparatorChar + "file.txt"));
                Assert.That(files[0].Content, Is.EqualTo("abd plk def tyu ghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        private string InitDirectory(IEnumerable<FileInfo> files)
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

        private IEnumerable<FileInfo> ReadDirectory(string path)
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
