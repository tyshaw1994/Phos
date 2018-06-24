using Newtonsoft.Json;

namespace Phos.Models
{
    public class PlexServer
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }
}