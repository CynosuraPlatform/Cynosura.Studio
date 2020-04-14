using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cynosura.Studio.CliTool.FunctionalTests
{
    public class NewCommandTest: IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _directoryName;
        public const string ProjectName = "Cynosura.TestProject";
        public NewCommandTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _directoryName = Guid.NewGuid().ToString("N");
        }
        
        [Fact]
        public async Task NewSolution()
        {
            var outBuffer = new StringWriter();
            Console.SetOut(outBuffer);

            // Act
            await Program.Main($"new {ProjectName} --solution {_directoryName}".Split(" "));

            var path = Path.Combine(Environment.CurrentDirectory, _directoryName);
            _testOutputHelper.WriteLine($"Project directory: {path}");
            _testOutputHelper.WriteLine($"cli output: {outBuffer.GetStringBuilder()}");
            
            // Assert
            Assert.True(Directory.Exists(path), "Directory.Exists(_directoryName)");
            Assert.True(File.Exists(Path.Combine(path, $"{ProjectName}.sln")), "File.Exists(Path.Combine(path, $'{ProjectName}.sln'))");
            Assert.True(outBuffer.GetStringBuilder().ToString().Contains($"Solution {ProjectName} created"),
                "outBuffer.GetStringBuilder().ToString().Contains($'Solution {ProjectName} created')");
        }

        public void Dispose()
        {
            var path = Path.Combine(Environment.CurrentDirectory, _directoryName);
            if (Directory.Exists(path))
            {
                _testOutputHelper.WriteLine($"Deleting path: {path}");
                Directory.Delete(path, true);
                _testOutputHelper.WriteLine($"successfully deleted {path}");
            }
        }
    }
}
