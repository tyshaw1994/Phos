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
using Phos.Converters;

namespace Phos.Managers
{
    public static class MyAnimeListManager
    {
        private static readonly string MalApiBaseUrl = "https://myanimelist.net/api/animelist";
        private static readonly string MalApiSearchBaseUrl = $"https://myanimelist.net/api/anime/search.xml?q=";
        private static readonly string MalAuthUrl = "https://myanimelist.net/api/account/verify_credentials.xml";
        private static readonly string MalListUrl = "https://myanimelist.net/malappinfo.php?u={0}&status=all&type=anime";

        public static Anime SearchListForShow(string title)
        {
            var username = ConfigurationManager.AppSettings["MalUserName"];

            var searchRequest = WebRequest.Create(string.Format(MalListUrl, username)) as HttpWebRequest;
            searchRequest.Method = "GET";

            var searchResponse = (HttpWebResponse)searchRequest.GetResponse();

            string animeList = string.Empty;
            using (var reader = new System.IO.StreamReader(searchResponse.GetResponseStream(), ASCIIEncoding.ASCII))
            {
                // this is XML
                animeList = reader.ReadToEnd();
            }

            var responseAsXml = new XmlDocument();
            responseAsXml.LoadXml(animeList);
            var responseAsJson = JsonConvert.SerializeXmlNode(responseAsXml.GetElementsByTagName("myanimelist")[0]);
            var malList = JsonConvert.DeserializeObject<MyAnimeList>(responseAsJson);

            var currentlyWatching = from show in malList.AnimeList.AllAnime
                                    where show.MyStatus == MalStatus.Watching
                                    select show;

            var currentShow = (from show in currentlyWatching
                              where TitleComparer.Compute(show.Title, title) < 5
                              select show).First();

            if(string.IsNullOrEmpty(currentShow.Title))
            {
                Logger.CreateLogEntry(LogType.Error, "MAL Search was not successful.", DateTime.Now);
                throw new Exception("MAL show was not found successfully.");
            }
            
            Logger.CreateLogEntry(LogType.Success, $"MAL API search was successful for {title}. ID: {currentShow.Id} | Total Episodes: {currentShow.Episodes}", DateTime.Now);

            return currentShow;
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
