using Newtonsoft.Json;

namespace Phos.Models
{
    public class PlexWriter
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("filter")]
        public string Filter { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}