using System.Collections.Generic;
using System.Text;
using AutoFixture.Xunit2;
using Xunit;

namespace Cynosura.Studio.CliTool.UnitTests
{
    public class ConfigServiceOverrideSettingsValueTests
    {
        [Theory]
        [InlineData("key1=value1")]
        [InlineData("key123sdf=value1ASD3f.sdfsdf=sdf")]
        public void OverrideSettingsValueBasic(string expression)
        {
            var service = new ConfigService();
            var props = service.OverrideSettingsValue(expression);
            Assert.Single(props);
        }

        [Theory]
        [InlineData("key1=value1,key2=value2")]
        [InlineData("key1=value1,key1.sub1=subvalue11")]
        [InlineData("key1=value1,key1.sub1.subsub1=subvalue11")]
        public void OverrideSettingsValueTwo(string expression)
        {
            var service = new ConfigService();
            var props = service.OverrideSettingsValue(expression);
            Assert.Equal(2, props.Count);
        }

        [Fact]
        public void OverrideSettingsValueSub()
        {
            const string key = "key1.sub1.subsub1";
            const string value = "sdfsdf";
            var parsedKey = key.Replace(".", ":");
            var service = new ConfigService();
            var props = service.OverrideSettingsValue($"{key}={value}");
            Assert.Single(props);
            Assert.True(props[parsedKey] == value, "props[parsedKey] == value");
        }

        [Fact]
        public void OverrideSettingsValueEscape()
        {
            const string key = "\"key1.s=ub1.subsub1\"";
            const string value = "sdfsdf";
            var parsedKey = key.Replace(".", ":");
            var service = new ConfigService();
            var props = service.OverrideSettingsValue($"{key}={value}");
            Assert.Single(props);
            Assert.True(props[parsedKey] == value, "props[parsedKey] == value");
        }


        [Fact]
        public void OverrideSettingsValueEscapeEscape()
        {
            const string key = "\"key1.s=u\\\"b1.sub,sub1\"";
            const string value = "sdfsdf";
            var parsedKey = key.Replace(".", ":");
            var service = new ConfigService();
            var props = service.OverrideSettingsValue($"{key}={value}");
            Assert.Single(props);
            Assert.True(props[parsedKey] == value, "props[parsedKey] == value");
        }

        [Fact]
        public void OverrideSettingsValueTwoFact()
        {
            const string key1 = "key1";
            const string value1 = "sdfsdfasdasgs";

            const string key2 = "key2";
            const string value2 = "sdfsdfasd";

            var service = new ConfigService();
            var props = service.OverrideSettingsValue($"{key1}={value1},{key2}={value2}");
            Assert.True(props[key1] == value1, "props[key1] == value1");
            Assert.True(props[key2] == value2, "props[key2] == value2");
        }
    }
}
