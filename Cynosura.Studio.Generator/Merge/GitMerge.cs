using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.Generator.Merge
{
    public class GitMerge : IDirectoryMerge
    {
        private static string[] _copyFilesIgnores = new[] { ".git", ".vs", "bin", "obj", "Debug", "Release", "node_modules" };

        private readonly ILogger<GitMerge> _logger;

        public GitMerge(ILogger<GitMerge> logger)
        {
            _logger = logger;
        }

        public async Task MergeDirectoryAsync(string originalDirectoryPath, string theirDirectoryPath, string myDirectoryPath, IEnumerable<(string Original, string Their)> renames = null)
        {
            _logger.LogInformation("Starting MergeDirectoryAsync for {Path}", myDirectoryPath);

            var repositoryPath = GetTempPath();

            await InitializeRepository(repositoryPath);

            await CommitOriginal(repositoryPath, originalDirectoryPath);

            await CommitUpgrade(repositoryPath, theirDirectoryPath);

            var exportPath = GetTempPath();

            ExportCurrent(myDirectoryPath, exportPath);

            await CommitCurrent(repositoryPath, exportPath);

            await MergeUpgrade(repositoryPath);

            Sync(exportPath, repositoryPath, myDirectoryPath);

            try
            {
                FileHelper.DeleteDirectory(exportPath);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, $"Can not delete {exportPath}");
            }
            try
            {
                FileHelper.DeleteDirectory(repositoryPath);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, $"Can not delete {repositoryPath}");
            }

            _logger.LogInformation("Completed MergeDirectoryAsync for {Path}", myDirectoryPath);
        }

        private async Task InitializeRepository(string repositoryPath)
        {
            Directory.CreateDirectory(repositoryPath);
            await RunCommandAsync("git", "init", repositoryPath);
        }

        private async Task CommitOriginal(string repositoryPath, string originalDirectoryPath)
        {
            FileHelper.CopyAllFiles(originalDirectoryPath, repositoryPath);
            await RunCommandAsync("git", "add -A", repositoryPath);
            await RunCommandAsync("git", "commit -m \"Original\"", repositoryPath);
        }

        private async Task CommitUpgrade(string repositoryPath, string upgradeDirectoryPath)
        {
            await RunCommandAsync("git", "checkout -b upgrade", repositoryPath);
            FileHelper.DeleteAllFiles(repositoryPath, ".git");
            FileHelper.CopyAllFiles(upgradeDirectoryPath, repositoryPath);
            await RunCommandAsync("git", "add -A", repositoryPath);
            await RunCommandAsync("git", "commit -m \"Upgrade\"", repositoryPath);
            await RunCommandAsync("git", "checkout master", repositoryPath);
        }

        private void ExportCurrent(string currentDirectoryPath, string exportPath)
        {
            Directory.CreateDirectory(exportPath);
            FileHelper.CopyAllFiles(currentDirectoryPath, exportPath, _copyFilesIgnores);
        }

        private async Task CommitCurrent(string repositoryPath, string currentDirectoryPath)
        {
            FileHelper.DeleteAllFiles(repositoryPath, ".git");
            FileHelper.CopyAllFiles(currentDirectoryPath, repositoryPath);
            await RunCommandAsync("git", "add -A", repositoryPath);
            await RunCommandAsync("git", "commit -m \"Current\"", repositoryPath);
        }

        private async Task MergeUpgrade(string repositoryPath)
        {
            await RunCommandAsync("git", "merge upgrade", repositoryPath);
        }

        private void Sync(string originalDirectoryPath, string theirDirectoryPath, string myDirectoryPath)
        {
            var compareFiles = DirectoryCompareHelper.Compare(originalDirectoryPath, theirDirectoryPath, ignores: new[] { $".git{Path.DirectorySeparatorChar}" });
            foreach (var compareFile in compareFiles)
            {
                var myFilePath = Path.Combine(myDirectoryPath, compareFile.OriginalName);
                if (compareFile.LeftPath == null)
                {
                    EnsureDirectoryExists(myFilePath);
                    File.Copy(compareFile.RightPath, myFilePath, true);
                }
                else if (compareFile.RightPath == null)
                {
                    if (File.Exists(myFilePath))
                        File.Delete(myFilePath);
                }
                else
                {
                    var leftFileInfo = new FileInfo(compareFile.LeftPath);
                    var rightFileInfo = new FileInfo(compareFile.RightPath);
                    if (leftFileInfo.LastWriteTimeUtc != rightFileInfo.LastWriteTimeUtc)
                    {
                        File.Copy(compareFile.RightPath, myFilePath, true);
                    }
                }
            }
        }

        private void EnsureDirectoryExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        private async Task<int> RunCommandAsync(string command, string arguments, string workingDirectory)
        {
            _logger.LogInformation("Running command {Command} {Arguments} in {WorkingDirectory}", command, arguments, workingDirectory);

            var psi = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                FileName = command,
                Arguments = arguments,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using var process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };
            return await RunCommandAsync(process).ConfigureAwait(false);
        }

        private Task<int> RunCommandAsync(Process process)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode);
            };
            process.OutputDataReceived += (s, ea) =>
            {
                if (!string.IsNullOrEmpty(ea.Data))
                {
                    _logger.LogInformation(ea.Data);
                }
            };
            process.ErrorDataReceived += (s, ea) =>
            {
                if (!string.IsNullOrEmpty(ea.Data))
                {
                    _logger.LogError(ea.Data);
                }
            };

            var started = process.Start();

            if (!started)
            {
                throw new InvalidOperationException("Could not run process: " + process);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }

        private string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }
    }
}
