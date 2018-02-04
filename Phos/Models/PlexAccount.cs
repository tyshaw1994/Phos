using Newtonsoft.Json;

namespace Phos.Models
{
    public class PlexAccount
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}