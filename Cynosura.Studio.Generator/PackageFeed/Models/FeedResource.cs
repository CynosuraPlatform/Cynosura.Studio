using Newtonsoft.Json;

namespace Cynosura.Studio.Generator.PackageFeed.Models
{
    public class FeedResource
    {
        [JsonProperty(PropertyName = "@id")]
        public string Id { get; set; } = null!;

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = null!;

        public string? Comment { get; set; }
    }
}
