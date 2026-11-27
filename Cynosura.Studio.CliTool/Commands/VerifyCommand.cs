using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool.Commands
{
    public class VerifyCommand : AppCommand
    {
        public VerifyCommand(string solutionDirectory, string feed, string src, string templateName, ServiceProvider serviceProvider)
            : base(solutionDirectory, feed, src, templateName, serviceProvider)
        {
        }

        public override async Task<bool> ExecuteAsync(string[] args)
        {
            var solutionFile = Directory.GetFiles(SolutionDirectory, "*.sln").FirstOrDefault();
            if (solutionFile == null)
            {
                Console.WriteLine($"No .sln file found in {SolutionDirectory}");
                return false;
            }

            Console.WriteLine($"Running 'dotnet build {Path.GetFileName(solutionFile)}' ...");
            var exitCode = await RunDotNetBuildAsync(SolutionDirectory, Path.GetFileName(solutionFile));
            Console.WriteLine(exitCode == 0 ? "Build succeeded." : "Build FAILED.");
            return exitCode == 0;
        }

        private static async Task<int> RunDotNetBuildAsync(string workingDirectory, string solutionFile)
        {
            var psi = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                FileName = "dotnet",
                Arguments = $"build \"{solutionFile}\"",
                UseShellExecute = false,
            };
            using var process = Process.Start(psi);
            if (process == null)
            {
                Console.WriteLine("Failed to start 'dotnet'. Is the .NET SDK installed?");
                return -1;
            }
            await process.WaitForExitAsync();
            return process.ExitCode;
        }

        public override string Help()
        {
            return $"{CliApp.CommandName} verify\r\n" +
                   $"Runs 'dotnet build' on the solution to verify the generated code compiles.\r\n";
        }
    }
}
