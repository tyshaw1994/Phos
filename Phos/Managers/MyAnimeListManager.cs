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
using System.IO;

namespace Phos.Managers
{
    public static class MyAnimeListManager
    {
        private static readonly string JikanApi = "https://api.jikan.me";
        private static readonly string MalApiBaseUrl = "https://myanimelist.net/api/animelist";
        private static readonly string MalAuthUrl = "https://myanimelist.net/api/account/verify_credentials.xml";

        public static async Task<JikanShow> SearchForShow(string title)
        {
            string showPayload = string.Empty;

            try
            {
                //TODO (Tyler): Figure out why this isn't working anymore
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
            var jikanShow = jikanResponse.Results[0];
            var malId = jikanShow.Id;

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

            Logger.CreateLogEntry(LogType.Success, $"Jikan search was successful for {title}. ID: {malId} | Total Episodes: {jikanShow.Episodes}", DateTime.Now);

            return jikanResponse.Results[0];
        }

        public static bool UpdateList(int malId, int episode, bool isFinished = false)
        {
            string username = ConfigurationManager.AppSettings["MalUserName"];
            string password = ConfigurationManager.AppSettings["MalPassword"];

            if(!ValidateMalCredentials(username, password))
            {
                Logger.CreateLogEntry(LogType.Error, "Failed to verify MAL credentials.", DateTime.UtcNow);
                return false;

            }

            // try to add the show to the list if it isn't already added
            MalAnimeValues add = new MalAnimeValues
            {
                Episode = episode,
                Status = (isFinished) ? (int)Enumerations.MalStatus.Completed : (int)Enumerations.MalStatus.Watching
            };
            string json = JsonConvert.SerializeObject(add);
            XmlDocument doc = JsonConvert.DeserializeXmlNode(json, "entry");

            HttpWebRequest addRequest = WebRequest.Create($"{MalApiBaseUrl}/add/{malId}.xml?data={doc.OuterXml}") as HttpWebRequest;
            addRequest.Method = "GET";
            addRequest.Headers["Authorization"] = "Basic " +
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            addRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse addResponse = (HttpWebResponse)addRequest.GetResponse();
            }
            catch (Exception)
            {
                // MAL will return a BadRequest if you already have the item on your list, so try to update it before failing. 
                HttpWebRequest updateRequest = WebRequest.Create($"{MalApiBaseUrl}/update/{malId}.xml?data={doc.OuterXml}") as HttpWebRequest;
                updateRequest.Method = "GET";
                updateRequest.Headers["Authorization"] = "Basic " +
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                updateRequest.ContentType = "application/x-www-form-urlencoded";

                try
                {
                    HttpWebResponse updateResponse = (HttpWebResponse)updateRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    Logger.CreateLogEntry(LogType.Error, ex, createdOn: DateTime.Now);
                    return false;
                }
            }

            Logger.CreateLogEntry(LogType.Success, $"Successfully updated show on {username}'s list.", DateTime.Now);
            return true;
        }

        private static bool ValidateMalCredentials(string username, string password)
        {
            HttpWebRequest searchRequest = WebRequest.Create($"{MalAuthUrl}") as HttpWebRequest;
            searchRequest.Headers["Authorization"] = "Basic " +
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            HttpWebResponse searchResponse = (HttpWebResponse)searchRequest.GetResponse();

            if (searchResponse.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
