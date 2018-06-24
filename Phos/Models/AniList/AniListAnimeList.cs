using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phos.Models.AniList
{
    public class AniListSearchResponse
    {
        [JsonProperty("data")]
        public GraphQlData Data { get; set; }
    }

    public class GraphQlData
    {
        [JsonProperty("MediaListCollection")]
        public GraphQlLists MediaListCollection { get; set; }
    }

    public class GraphQlLists
    {
        [JsonProperty("lists")]
        public List<GraphQlEntry> Lists { get; set; }
    }

    public class GraphQlEntry
    {
        [JsonProperty("entries")]
        public List<AniListAnime> Entries { get; set; }
    }
}