using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phos.Models
{
    public class JikanResponse
    {
        [JsonProperty("result")]
        public JikanShow[] Results { get; set; }
    }
}