using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cynosura.Studio.Generator.Merge;
using Xunit;

namespace Cynosura.Studio.Generator.Tests
{
    public class DirectoryCompareHelperTests
    {
        [Fact]
        public void Compare_Simple()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def tyu ghi") });
            
            try
            {
                var compares = DirectoryCompareHelper.Compare(dir1, dir2);
                Assert.Equal(1, compares.Count);
                Assert.Equal("file.txt", compares[0].Name);
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
            }
        }

        [Fact]
        public void Compare_Subdirectories()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path/file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new FileHelper.FileInfo[0]);

            try
            {
                var compares = DirectoryCompareHelper.Compare(dir1, dir2);
                Assert.Equal(1, compares.Count);
                Assert.Equal($"path{Path.DirectorySeparatorChar}file.txt", compares[0].Name);
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
            }
        }

        [Fact]
        public void Compare_WithIgnore()
        {
            var dir1 = FileHelper.InitDirectory(new[] { 
                new FileHelper.FileInfo("path/file.txt", "abd def ghi"),
                new FileHelper.FileInfo("path2/file.txt", "abd def ghi")
            });
            var dir2 = FileHelper.InitDirectory(new FileHelper.FileInfo[0]);

            try
            {
                var compares = DirectoryCompareHelper.Compare(dir1, dir2, ignores: new[] { $"path{Path.DirectorySeparatorChar}" });
                Assert.Equal(1, compares.Count);
                Assert.Equal($"path2{Path.DirectorySeparatorChar}file.txt", compares[0].Name);
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
            }
        }
    }
}
