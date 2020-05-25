using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Generator
{
    public static class FileHelper
    {
        public static void CopyAllFiles(string sourceDirectory, string destinationDirectory, IList<string> ignores = null, bool overwrite = false)
        {
            var dir = new DirectoryInfo(sourceDirectory);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirectory);
            }

            var dirs = dir.GetDirectories();

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            var files = dir.GetFiles();
            foreach (var file in files)
            {
                if (ignores != null && ignores.Contains(file.Name))
                {
                    continue;
                }
                var destinationPath = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(destinationPath, overwrite);
            }

            foreach (var subdir in dirs)
            {
                if (ignores != null && ignores.Contains(subdir.Name))
                {
                    continue;
                }
                var destinationPath = Path.Combine(destinationDirectory, subdir.Name);
                CopyAllFiles(subdir.FullName, destinationPath, ignores);
            }
        }

        public static void DeleteAllFiles(string directory, string except)
        {
            var dir = new DirectoryInfo(directory);

            foreach (var file in dir.GetFiles())
            {
                if (file.Name == except)
                    continue;
                file.Delete();
            }
            foreach (var subdir in dir.GetDirectories())
            {
                if (subdir.Name == except)
                    continue;
                subdir.Delete(true);
            }
        }

        public static void DeleteDirectory(string path)
        {
            var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }

            directory.Delete(true);
        }

        public static async Task<string> ReadFileAsync(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return await fileReader.ReadToEndAsync();
            }
        }

        public static async Task WriteFileAsync(string filePath, string content)
        {
            using (var fileWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await fileWriter.WriteAsync(content);
            }
        }

        public static async Task InsertTextAsync(string filePath, string content, string insertAfter, string fileSavePath)
        {
            var fileContent = await ReadFileAsync(filePath);

            if (!fileContent.Contains(content))
            {
                fileContent = fileContent.Replace(insertAfter + "\r\n",
                    insertAfter + "\r\n" + content + "\r\n");

                if (!fileContent.Contains(content))
                {
                    fileContent = fileContent.Replace(insertAfter + "\n",
                        insertAfter + "\n" + content + "\n");
                }

                await WriteFileAsync(fileSavePath, fileContent);
            }
        }

        public static async Task RemoveTextAsync(string filePath, string content)
        {
            var fileContent = await ReadFileAsync(filePath);

            if (fileContent.Contains(content))
            {
                fileContent = fileContent.Replace(content + "\r\n", "");

                if (fileContent.Contains(content))
                {
                    fileContent = fileContent.Replace(content + "\n", "");
                }

                await WriteFileAsync(filePath, fileContent);
            }
        }
    }
}
