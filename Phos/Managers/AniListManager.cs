using Newtonsoft.Json;
using Phos.Converters;
using Phos.Enumerations;
using Phos.Logging;
using Phos.Models;
using Phos.Models.AniList;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phos.Managers
{
    public static class AniListManager
    {
        public static string BaseUrl = "https://graphql.anilist.co/";
        public static string OAuthUrl = "https://anilist.co/api/v2/oauth/authorize";

        public static async Task<bool> UpdateListAsync(RegisterValues values, PlexRequest plexRequest)
        {
            var entries = await GetListForUser(values.UserName);
            var title = plexRequest.Metadata.GrandparentTitle;

            AniListAnime currentShow = new AniListAnime();
            try
            {
                currentShow = entries.Entries
                    .Where(x => TitleComparer.Compute(x.MediaItem.Title.UserPreferred, title) < 5 ||
                    x.MediaItem.Title.UserPreferred.ToLower().Contains(title.ToLower())).First();
            }
            catch
            {
                try
                {
                    currentShow = entries.Entries.Select(x => x)
                        .OrderByDescending(x => TitleComparer.Compute(x.MediaItem.Title.UserPreferred.ToLower(), title.ToLower()))
                        .First();
                }
                catch (Exception ex)
                {
                    Logger.CreateLogEntry(LogType.Error, ex, DateTime.Now);
                    currentShow = null;
                }
            }

            if (currentShow == null) return false;

            var query = FormUpdateQuery(currentShow.EntryId, currentShow.MediaItem.Id, plexRequest.Metadata.Index);

            var token = values.AniListAccessToken;
            HttpWebRequest request = WebRequest.Create(BaseUrl + query) as HttpWebRequest;
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + token;

            AniListSearchResponse aniListResponse;
            try
            {
                var response = (HttpWebResponse)await request.GetResponseAsync();

                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    var aniListResponseString = reader.ReadToEnd();
                    aniListResponse = JsonConvert.DeserializeObject<AniListSearchResponse>(aniListResponseString);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static async Task<GraphQlEntry> GetListForUser(string userName)
        {
            var query = "?query=query{MediaListCollection(userName:\"SoraX64\",status:CURRENT,type:ANIME){lists{entries{id,media{id,episodes,title{romaji,english,userPreferred}}}}}}";

            HttpWebRequest request = WebRequest.Create(BaseUrl + query) as HttpWebRequest;
            request.Method = "POST";

            AniListSearchResponse aniListResponse;
            try
            {
                var response = (HttpWebResponse)await request.GetResponseAsync();

                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    var aniListResponseString = reader.ReadToEnd();
                    aniListResponse = JsonConvert.DeserializeObject<AniListSearchResponse>(aniListResponseString);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return aniListResponse.Data.MediaListCollection.Lists[0];
        }

        private static string FormUpdateQuery(int entryId, int mediaId, int episode)
        {
            string queryStart = "?query=mutation{SaveMediaListEntry(";
            string queryMiddle = $"id:{entryId},mediaId:{mediaId},progress:{episode}";
            string queryEnd = "){id}}";

            return queryStart + queryMiddle + queryEnd;
        }
    }
}