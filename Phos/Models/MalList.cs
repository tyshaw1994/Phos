using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phos.Models
{
    public class MalList
    {
        [JsonProperty("myinfo")]
        public MyInfo UserInfo { get; set; }

        [JsonProperty("anime")]
        public Anime[] AllAnime { get; set; }
    }

    public class MyAnimeList
    {
        [JsonProperty("myanimelist")]
        public MalList AnimeList { get; set; }
    }
}