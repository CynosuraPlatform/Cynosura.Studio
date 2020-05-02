using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Merge;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Cynosura.Studio.Generator.Tests
{
    public class GitMergeTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public GitMergeTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private string Normalize(string str)
        {
            return str.Replace("\r\n", "\n");
        }

        private ILogger<T> GetLoggerMock<T>()
        {
            return new FakeLogger<T>(_testOutputHelper);
        }

        [Fact]
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
                Assert.Single(files);
                Assert.Equal("file.txt", files[0].Path);
                Assert.Equal("abd\nplk\ndef\ntyu\nghi", Normalize(files[0].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Equal(2, files.Count);
                Assert.Equal("file2.txt", files[1].Path);
                Assert.Equal("abd def tyu ghi", Normalize(files[1].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Single(files);
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Equal(2, files.Count);
                Assert.Equal("file2.txt", files[1].Path);
                Assert.Equal("abd def poi ghi", Normalize(files[1].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact] 
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
                Assert.Equal(2, files.Count);
                Assert.Equal("file2.txt", files[1].Path);
                Assert.Equal("abd def tyu ghi", Normalize(files[1].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Single(files);
                Assert.Equal("file2.txt", files[0].Path);
                Assert.Equal("abd\nplk\ndef\ntyu\nghi", Normalize(files[0].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Single(files);
                Assert.Equal("file2.txt", files[0].Path);
                Assert.Equal("abd\nplk\ndef\ntyu\nghi", Normalize(files[0].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Single(files);
                Assert.Equal("file2.txt", files[0].Path);
                Assert.Equal("abd plk def ghi", Normalize(files[0].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Single(files);
                Assert.Equal("path2" + Path.DirectorySeparatorChar + "file.txt", files[0].Path);
                Assert.Equal("abd\nplk\ndef\ntyu\nghi", Normalize(files[0].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Single(files);
                Assert.Equal("file.txt", files[0].Path);
                Assert.Equal("abd\nplk\ndef\nghi", Normalize(files[0].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact]
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
                Assert.Single(files);
                Assert.Equal("file.txt", files[0].Path);
                Assert.Equal("abd\ndef\ntyu\nghi", Normalize(files[0].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        class FakeLogger<T> : ILogger<T>
        {
            private readonly ITestOutputHelper _testOutputHelper;

            public FakeLogger(ITestOutputHelper testOutputHelper)
            {
                _testOutputHelper = testOutputHelper;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                _testOutputHelper.WriteLine(formatter(state, exception));
            }
        }
    }


}
