using Newtonsoft.Json;

namespace Cynosura.Studio.Generator.PackageFeed.Models
{
    public class FeedResource
    {
        [JsonProperty(PropertyName = "@id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; }

        public string Comment { get; set; }
    }
}
