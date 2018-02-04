using Newtonsoft.Json;

namespace Phos.Models
{
    public class PlexDirector
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("filter")]
        public string Filter { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}