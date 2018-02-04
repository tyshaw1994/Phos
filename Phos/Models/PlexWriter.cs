using Newtonsoft.Json;

namespace Phos.Models
{
    public class PlexWriter
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("filter")]
        public string FIlter { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}