using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Phos.Models
{
    public class MalAnimeValues
    {
        [JsonProperty("episode")]
        public int Episode { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("storage_type")]
        public int? StorageType { get; set; }

        [JsonProperty("storage_value")]
        public float? StorageValue { get; set; }

        [JsonProperty("times_rewatched")]
        public int? TimesRewatched { get; set; }

        [JsonProperty("date_start")]
        public string DateStart { get; set; }

        [JsonProperty("date_finish")]
        public string DateFinish { get; set; }

        [JsonProperty("priority")]
        public int? Priority { get; set; }

        [JsonProperty("enable_discussion")]
        public int? EnableDiscussion { get; set; }

        [JsonProperty("enable_rewatching")]
        public int? EnableRewatching { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }
    }
}