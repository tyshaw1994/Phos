using Newtonsoft.Json;
using Phos.Converters;
using Phos.Enumerations;
using System;

namespace Phos.Models
{
    public class MalAnime
    {
        [JsonProperty("series_animedb_id")]
        public int Id { get; set; }

        [JsonProperty("series_title")]
        public string Title { get; set; }

        [JsonProperty("series_synonyms")]
        public string Synonyms { get; set; }

        [JsonProperty("series_type")]
        public AnimeType ShowType { get; set; }

        [JsonProperty("series_episodes")]
        public int Episodes { get; set; }

        [JsonProperty("series_status")]
        public MalStatus Status { get; set; }

        [JsonProperty("series_start")]
        public string StartDate { get; set; }

        [JsonProperty("series_end")]
        public string EndDate { get; set; }

        [JsonProperty("series_image")]
        public string Image { get; set; }

        [JsonProperty("my_id")]
        public int MyId { get; set; }

        [JsonProperty("my_watched_episodes")]
        public int MyWatchedEpisodes { get; set; }

        [JsonProperty("my_start_date")]
        public string MyStartDate { get; set; }

        [JsonProperty("my_end_date")]
        public string MyEndDate { get; set; }

        [JsonProperty("my_score")]
        public int MyScore { get; set; }

        [JsonProperty("my_status")]
        public MalStatus MyStatus { get; set; }

        [JsonProperty("my_rewatching")]
        public string Rewatching { get; set; }

        [JsonProperty("my_rewatching_ep")]
        public int RewatchingEpisode { get; set; }

        [JsonProperty("my_last_updated"), JsonConverter(typeof(EpochToDateTimeConverter))]
        public DateTime MyLastUpdated { get; set; }

        [JsonProperty("my_tags")]
        public string MyTags { get; set; }
    }
}