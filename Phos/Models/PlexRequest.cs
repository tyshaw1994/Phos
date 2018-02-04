using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phos.Models
{
    public class PlexRequest
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("user")]
        public bool User { get; set; }

        [JsonProperty("owner")]
        public bool Owner { get; set; }

        [JsonProperty("Account")]
        public PlexAccount Account { get; set; }

        [JsonProperty("Server")]
        public PlexServer Server { get; set; }

        [JsonProperty("Player")]
        public PlexPlayer Player { get; set; }

        [JsonProperty("Metadata")]
        public PlexMetadata Metadata { get; set; }

    }
}