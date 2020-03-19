using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Merge;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Cynosura.Studio.Generator.Tests
{
    [TestFixture]
    public class GitMergeTests
    {
        private string Normalize(string str)
        {
            return str.Replace("\r\n", "\n");
        }

        private ILogger<T> GetLoggerMock<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }

        [Test]
        public async Task MergeDirectoryAsync_SimpleMerge()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\ntyu\nghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\nplk\ndef\nghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file.txt"));
                Assert.That(Normalize(files[0].Content), Is.EqualTo("abd\nplk\ndef\ntyu\nghi"));
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa"), new FileHelper.FileInfo("file2.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(2));
                Assert.That(files[1].Path, Is.EqualTo("file2.txt"));
                Assert.That(Normalize(files[1].Content), Is.EqualTo("abd def tyu ghi"));
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa"), new FileHelper.FileInfo("file2.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa"), new FileHelper.FileInfo("file2.txt", "abd def ghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_DoNotDeleteChanged()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa"), new FileHelper.FileInfo("file2.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa"), new FileHelper.FileInfo("file2.txt", "abd def poi ghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(2));
                Assert.That(files[1].Path, Is.EqualTo("file2.txt"));
                Assert.That(Normalize(files[1].Content), Is.EqualTo("abd def poi ghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test] 
        public async Task MergeDirectoryAsync_ChangeDeleted()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa"), new FileHelper.FileInfo("file2.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa"), new FileHelper.FileInfo("file2.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file1.txt", "aaaa") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(2));
                Assert.That(files[1].Path, Is.EqualTo("file2.txt"));
                Assert.That(Normalize(files[1].Content), Is.EqualTo("abd def tyu ghi"));
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file2.txt", "abd\ndef\ntyu\nghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\nplk\ndef\nghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file2.txt"));
                Assert.That(Normalize(files[0].Content), Is.EqualTo("abd\nplk\ndef\ntyu\nghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_MergeWithAlreadyRenamed()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file2.txt", "abd\ndef\ntyu\nghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file2.txt", "abd\nplk\ndef\nghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file2.txt"));
                Assert.That(Normalize(files[0].Content), Is.EqualTo("abd\nplk\ndef\ntyu\nghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_RenameWithoutChange()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file2.txt", "abd def ghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file2.txt"));
                Assert.That(Normalize(files[0].Content), Is.EqualTo("abd plk def ghi"));
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path1/file.txt", "abd\ndef\nghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path2/file.txt", "abd\ndef\ntyu\nghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path1/file.txt", "abd\nplk\ndef\nghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("path2" + Path.DirectorySeparatorChar + "file.txt"));
                Assert.That(Normalize(files[0].Content), Is.EqualTo("abd\nplk\ndef\ntyu\nghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_NoChange()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\nplk\ndef\nghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file.txt"));
                Assert.That(Normalize(files[0].Content), Is.EqualTo("abd\nplk\ndef\nghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Test]
        public async Task MergeDirectoryAsync_NoMerge()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\ntyu\nghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            try
            {
                var fileMerge = new GitMerge(GetLoggerMock<GitMerge>());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.That(files.Count, Is.EqualTo(1));
                Assert.That(files[0].Path, Is.EqualTo("file.txt"));
                Assert.That(Normalize(files[0].Content), Is.EqualTo("abd\ndef\ntyu\nghi"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }
    }


}
