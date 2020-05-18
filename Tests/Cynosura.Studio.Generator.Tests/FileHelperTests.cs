using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cynosura.Studio.Generator.Tests
{
    public class FileHelperTests
    {
        private string Normalize(string str)
        {
            return str.Replace("\r\n", "\n");
        }

        [Fact]
        public async Task InsertTextAsync_SuccessWin()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\r\ndef\r\nghi") });
            var file1 = Path.Combine(dir1, "file.txt");
            var file2 = Path.Combine(dir1, "file2.txt");
            try
            {
                await Generator.FileHelper.InsertTextAsync(file1, "test", "def", file2);
                var files = FileHelper.ReadDirectory(dir1).ToList();
                Assert.Equal(2, files.Count);
                Assert.Equal("abd\r\ndef\r\ntest\r\nghi", files[1].Content);
            }
            finally
            {
                Directory.Delete(dir1, true);
            }
        }

        [Fact]
        public async Task InsertTextAsync_SuccessLinux()
        {
            var dir1 = FileHelper.InitDirectory(new[] { new FileHelper.FileInfo("file.txt", "abd\ndef\nghi") });
            var file1 = Path.Combine(dir1, "file.txt");
            var file2 = Path.Combine(dir1, "file2.txt");
            try
            {
                await Generator.FileHelper.InsertTextAsync(file1, "test", "def", file2);
                var files = FileHelper.ReadDirectory(dir1).ToList();
                Assert.Equal(2, files.Count);
                Assert.Equal("abd\ndef\ntest\nghi", Normalize(files[1].Content));
            }
            finally
            {
                Directory.Delete(dir1, true);
            }
        }
    }
}
