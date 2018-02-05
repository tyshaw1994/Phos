using System;
using Phos.Models;
using Phos.Logging;
using Phos.Enumerations;
using Newtonsoft.Json;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Linq;

namespace Phos.Managers
{
    public static class MyAnimeListManager
    {
        private static readonly string JikanApi = "https://api.jikan.me";

        public static async Task<JikanShow> SearchForShow(string title)
        {
            string showPayload = string.Empty;

            try
            {
                HttpWebRequest searchRequest = WebRequest.Create($"{JikanApi}/search/anime/{title.Replace(" ", "")}/1") as HttpWebRequest;
                HttpWebResponse searchResponse = (HttpWebResponse)searchRequest.GetResponse();

                using (var reader = new System.IO.StreamReader(searchResponse.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    showPayload = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logger.CreateLogEntry(LogType.Error, ex, DateTime.UtcNow);
            }

            var jikanResponse = JsonConvert.DeserializeObject<JikanResponse>(showPayload);
            var malId = jikanResponse.Results[0].Id;

            try
            {
                HttpWebRequest searchRequest = WebRequest.Create($"{JikanApi}/anime/{malId}/stats") as HttpWebRequest;
                HttpWebResponse searchResponse = (HttpWebResponse)searchRequest.GetResponse();

                using (var reader = new System.IO.StreamReader(searchResponse.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    showPayload = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logger.CreateLogEntry(LogType.Error, ex, DateTime.UtcNow);
            }

            return jikanResponse.Results[0];
        }
    }
}
