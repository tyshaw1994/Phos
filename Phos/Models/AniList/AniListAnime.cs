using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phos.Models.AniList
{
    public class AniListAnime
    {
        [JsonProperty("media")]
        public AniListMedia MediaItem { get; set; }

        [JsonProperty("id")]
        public int EntryId { get; set; }
    }

    public class AniListMedia
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("episodes")]
        public int? Episodes { get; set; }

        [JsonProperty("title")]
        public AniListMediaTitle Title { get; set; }
    }

    public class AniListMediaTitle
    {
        [JsonProperty("romaji")]
        public string Romaji { get; set; }

        [JsonProperty("english")]
        public string English { get; set; }

        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }
}