using System;
using Newtonsoft.Json;
using Phos.Enumerations;

namespace Phos.Models
{
    public class MalShow
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("english")]
        public string EnglishTitle { get; set; }

        [JsonProperty("synonyms")]
        public string Synonyms { get; set; }

        [JsonProperty("episodes")]
        public int Episodes { get; set; }

        [JsonProperty("type")]
        public AnimeType Type { get; set; }

        [JsonProperty("score")]
        public float Score { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }

        [JsonProperty("image")]
        public string ImageUrl { get; set; }
    }

    public class Entry
    {
        [JsonProperty("entry")]
        public MalShow Show { get; set; }
    }
}
