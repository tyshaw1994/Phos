using Newtonsoft.Json;
using Phos.Converters;
using System;

namespace Phos.Models
{
    public class PlexMetadata
    {
        [JsonProperty("librarySectionType")]
        public string LibrarySectionType { get; set; }

        [JsonProperty("ratingKey")]
        public string RatingKey { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("parentRatingKey")]
        public string ParentRatingKey { get; set; }

        [JsonProperty("grandparentRatingKey")]
        public string GrandparentRatingKey { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("librarySectionID")]
        public int LibrarySectionId { get; set; }

        [JsonProperty("librarySectionKey")]
        public string LibrarySectionKey { get; set; }

        [JsonProperty("librarySectionTitle")]
        public string LibrarySectionTitle { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("grandparentKey")]
        public string GrandparentKey { get; set; }

        [JsonProperty("parentKey")]
        public string ParentKey { get; set; }

        [JsonProperty("grandparentTitle")]
        public string GrandparentTitle { get; set; }

        [JsonProperty("parentTitle")]
        public string ParentTitle { get; set; }

        [JsonProperty("contentRating")]
        public string ContentRating { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("parentIndex")]
        public int ParentIndex { get; set; }

        [JsonProperty("viewOffset")]
        public int ViewOffset { get; set; }

        [JsonProperty("lastViewedAt"), JsonConverter(typeof(EpochToDateTimeConverter))]
        public DateTime LastViewedAt { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("ratingCount")]
        public int RatingCount { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }

        [JsonProperty("art")]
        public string Art { get; set; }

        [JsonProperty("parentThumb")]
        public string ParentThumb { get; set; }

        [JsonProperty("parentArt")]
        public string ParentArt { get; set; }

        [JsonProperty("parentTheme")]
        public string ParentTHeme { get; set; }

        [JsonProperty("grandparentThumb")]
        public string GrandparentThumb { get; set; }

        [JsonProperty("grandparentTheme")]
        public string GrandparentTheme { get; set; }

        [JsonProperty("grandparentArt")]
        public string GrandparentArt { get; set; }

        [JsonProperty("originallyAvailableAt")]
        public string OriginallyAvailableAt { get; set; }

        [JsonProperty("addedAt"), JsonConverter(typeof(EpochToDateTimeConverter))]
        public DateTime AddedAt { get; set; }

        [JsonProperty("updatedAt"), JsonConverter(typeof(EpochToDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("Director")]
        public PlexDirector Director { get; set; }

        [JsonProperty("Writer")]
        public PlexWriter Writer { get; set; }
    }
}