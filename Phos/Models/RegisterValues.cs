using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Phos.Enumerations;
using Phos.Logging;

namespace Phos.Models
{
    public class RegisterValues
    {
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("TwilioUserName")]
        public string TwilioUserName { get; set; }

        [JsonProperty("TwilioPassword")]
        public string TwilioPassword { get; set; }

        [JsonProperty("TwilioPhoneNumber")]
        public string TwilioPhoneNumber { get; set; }

        [JsonProperty("UserPhoneNumber")]
        public string UserPhoneNumber { get; set; }

        [JsonProperty("AniListAccessToken")]
        public string AniListAccessToken { get; set; }

        [JsonProperty("ClientId")]
        public string ClientId { get; set; }

        [JsonProperty("ClientSecret")]
        public string ClientSecret { get; set; }

        [JsonProperty("RedirectUri")]
        public string RedirectUri { get; set; }

        public static RegisterValues GetRegisteredValues()
        {
            RegisterValues values = new RegisterValues();

            try
            {
                using (StreamReader r = new StreamReader($"{ConfigurationManager.AppSettings["RootDirectory"]}creds.txt"))
                {
                    values = JsonConvert.DeserializeObject<RegisterValues>(r.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Logger.CreateLogEntry(LogType.Error, ex, DateTime.Now);
            }

            return values;
        }
    }
}