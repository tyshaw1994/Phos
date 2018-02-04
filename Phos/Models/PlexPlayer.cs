using Newtonsoft.Json;

namespace Phos.Models
{
    public class PlexPlayer
    {
        [JsonProperty("local")]
        public bool Local { get; set; }

        [JsonProperty("publicAddress")]
        public string PublicAddress { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }
}