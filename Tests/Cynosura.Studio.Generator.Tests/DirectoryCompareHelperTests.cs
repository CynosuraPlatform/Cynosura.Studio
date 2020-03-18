using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cynosura.Studio.Generator.Merge;
using NUnit.Framework;

namespace Cynosura.Studio.Generator.Tests
{
    [TestFixture]
    class DirectoryCompareHelperTests
    {
        [Test]
        public void Compare_Simple()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd def tyu ghi") });
            
            try
            {
                var compares = DirectoryCompareHelper.Compare(dir1, dir2);
                Assert.That(compares.Count, Is.EqualTo(1));
                Assert.That(compares[0].Name, Is.EqualTo("file.txt"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
            }
        }

        [Test]
        public void Compare_Subdirectories()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("path/file.txt", "abd def ghi") });
            var dir2 = FileHelper.InitDirectory(new FileHelper.FileInfo[0]);

            try
            {
                var compares = DirectoryCompareHelper.Compare(dir1, dir2);
                Assert.That(compares.Count, Is.EqualTo(1));
                Assert.That(compares[0].Name, Is.EqualTo($"path{Path.DirectorySeparatorChar}file.txt"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
            }
        }

        [Test]
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
                Assert.That(compares.Count, Is.EqualTo(1));
                Assert.That(compares[0].Name, Is.EqualTo($"path2{Path.DirectorySeparatorChar}file.txt"));
            }
            finally
            {
                Directory.Delete(dir1, true);
                Directory.Delete(dir2, true);
            }
        }
    }
}
