using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cynosura.Studio.Core
{
    public static class JsonExtensions
    {
        public static JsonSerializerSettings Settings => new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string SerializeToJson<T>(this T obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);
            return json;
        }

        public static T DeserializeFromJson<T>(this string json, params JsonConverter[] converters) where T : new()
        {
            var settings = Settings;
            foreach (var converter in converters)
            {
                settings.Converters.Add(converter);
            }
            var obj = JsonConvert.DeserializeObject<T>(json, settings);
            return obj;
        }
    }
}