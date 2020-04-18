using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Merge;
using Xunit;

namespace Cynosura.Studio.Generator.Tests
{
    public class DirectoryMergeTests
    {
        [Fact]
        public async Task MergeDirectoryAsync_SimpleMerge()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Single(files);
                Assert.Equal("file.txt", files[0].Path);
                Assert.Equal("abd plk def tyu ghi", files[0].Content);
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
            var dir1 = FileHelper.InitDirectory(new FileHelper.FileInfo[] { });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new FileHelper.FileInfo[] { });
            try
            {
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Single(files);
                Assert.Equal("file.txt", files[0].Path);
                Assert.Equal("abd def tyu ghi", files[0].Content);
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new FileHelper.FileInfo[] { });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Empty(files);
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
                Directory.Delete(dir3, true);
            }
        }

        [Fact] 
        public async Task MergeDirectoryAsync_MergeDeleted()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new FileHelper.FileInfo[] { });
            try
            {
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3);
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Empty(files);
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file2.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3,
                    new[] { ("file.txt", "file2.txt") });
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Single(files);
                Assert.Equal("file2.txt", files[0].Path);
                Assert.Equal("abd plk def tyu ghi", files[0].Content);
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file2.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file2.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3,
                    new[] { ("file.txt", "file2.txt") });
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Single(files);
                Assert.Equal("file2.txt", files[0].Path);
                Assert.Equal("abd plk def tyu ghi", files[0].Content);
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
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3,
                    new[] { ("file.txt", "file2.txt") });
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Single(files);
                Assert.Equal("file2.txt", files[0].Path);
                Assert.Equal("abd plk def ghi", files[0].Content);
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
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path1/file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path2/file.txt", "abd def tyu ghi") });
            var dir3 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path1/file.txt", "abd plk def ghi") });
            try
            {
                var fileMerge = new DirectoryMerge(new DmpMerge());
                await fileMerge.MergeDirectoryAsync(dir1, dir2, dir3,
                    new[] { ("path1", "path2") });
                var files = FileHelper.ReadDirectory(dir3).ToList();
                Assert.Single(files);
                Assert.Equal("path2" + Path.DirectorySeparatorChar + "file.txt", files[0].Path);
                Assert.Equal("abd plk def tyu ghi", files[0].Content);
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
