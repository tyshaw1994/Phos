using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phos.Models.AniList
{
    public class TokenInfo
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
    }
}