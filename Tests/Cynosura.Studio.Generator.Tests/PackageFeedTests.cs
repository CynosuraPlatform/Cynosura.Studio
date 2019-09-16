using System;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.PackageFeed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Cynosura.Studio.Generator.Tests
{
    [TestFixture]
    public class PackageFeedTests
    {
        [Test]
        public async Task FeedListing()
        {
            var config = GetConfig();
            var configMock = new Mock<IOptions<NugetSettings>>();
            configMock.Setup(s => s.Value).Returns(config);
            var feed = new NugetFeed(configMock.Object);
            var versions = await feed.GetVersionsAsync("Cynosura.Template");
            Assert.IsNotEmpty(versions);
        }
        
        private NugetSettings GetConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path: "appsettings.local.json", optional: true);
            var configuration = builder.Build();
            return configuration.GetSection("Nuget").Get<NugetSettings>();
        }
    }
}
