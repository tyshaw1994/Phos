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
using System.Diagnostics;

namespace Phos.Managers
{
    public static class MyAnimeListManager
    {
        private static readonly string MalApiBaseUrl = "https://myanimelist.net/api/animelist";
        private static readonly string MalApiSearchBaseUrl = $"https://myanimelist.net/api/anime/search.xml?q=";
        private static readonly string MalAuthUrl = "https://myanimelist.net/api/account/verify_credentials.xml";
        private static readonly string MalListUrl = "https://myanimelist.net/malappinfo.php?u={0}&status=all&type=anime";

        public static Anime SearchListForShow(string username, string title)
        {
            // Clean out any accented characters from the show title
            var tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(title);
            title = System.Text.Encoding.UTF8.GetString(tempBytes);

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

            Anime currentShow;

            try
            {
                currentShow = (from show in currentlyWatching
                               where (TitleComparer.Compute(show.Title, title) < 5 || show.Title.ToLower().Contains(title.ToLower()))
                               select show).First();
            }
            catch
            {
                //Usually the title from Plex is the English title and not the default Japanese title. Check the Synonyms.
                try
                {
                    currentShow = (from show in currentlyWatching
                                   where (TitleComparer.Compute(show.Synonyms, title) < 5) || show.Synonyms.ToLower().Replace("; ", ";").Split(';').Contains(title.ToLower())
                                   select show).First();
                }
                catch (Exception ex)
                {
                    Logger.CreateLogEntry(LogType.Error, ex, DateTime.Now);
                    currentShow = null;
                }
            }

            if(string.IsNullOrEmpty(currentShow.Title))
            {
                Logger.CreateLogEntry(LogType.Error, "MAL Search was not successful.", DateTime.Now);
                throw new Exception("MAL show was not found successfully.");
            }
            
            Logger.CreateLogEntry(LogType.Success, $"MAL API search was successful for {title}. ID: {currentShow.Id} | Total Episodes: {currentShow.Episodes}", DateTime.Now);

            return currentShow;
        }

        public static bool UpdateList(string username, string password, int malId, int episode, bool isFinished = false)
        {
            if (!ValidateMalCredentials(username, password))
            {
                Logger.CreateLogEntry(LogType.Error, "Failed to verify MAL credentials.", DateTime.UtcNow);
                return false;
            }

            MalAnimeValues add = new MalAnimeValues
            {
                Episode = episode,
                Status = (isFinished) ? (int)Enumerations.MalStatus.Completed : (int)Enumerations.MalStatus.Watching
            };
            string json = JsonConvert.SerializeObject(add);
            XmlDocument doc = JsonConvert.DeserializeXmlNode(json, "entry");

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
        

            Logger.CreateLogEntry(LogType.Success, $"Successfully updated show on {username}'s list.", DateTime.Now);
            return true;
        }

        public static bool RegisterCredentials(RegisterValues values)
        {
            try
            {
                using (StreamWriter file = File.CreateText($"{ConfigurationManager.AppSettings["RootDirectory"]}creds.txt"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, values);
                }
            }
            catch (Exception ex)
            {
                Logger.CreateLogEntry(LogType.Error, ex, DateTime.Now);
                return false;
            }

            return true;
        }

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
