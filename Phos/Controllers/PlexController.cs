using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Phos.Logging;
using Phos.Models;
using Phos.Managers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Plex.Server.Webhooks;
using Plex.Server.Webhooks.Converters;
using Plex.Server.Webhooks.Events.Core;
using Plex.Server.Webhooks.Events;
using Plex.Server.Webhooks.Service;

namespace Phos.Controllers
{
    public class PlexController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> PostWebhook()
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var content = await this.Request.Content.ReadAsStringAsync();
            var plexRequest = JsonConvert.DeserializeObject<PlexRequest>(PlexManager.ParseJsonFromWebhook(content));

            // TODO(Tyler): Figure out a way to utilize the other play events
            if (plexRequest.Event.Equals("media.scrobble"))
            {
                var show = await MyAnimeListManager.SearchForShow(plexRequest.Metadata.GrandparentTitle);

                if(!(show is JikanShow))
                {
                    Logger.CreateLogEntry(Enumerations.LogLevel.Failure, new ArgumentException("Show was not found through Jikan API search or some other error occured."), DateTime.Now);
                }

                var id = show.Id;
                var episodeCompleted = plexRequest.Metadata.Index;
            }



            Logger.CreateLogEntry(Enumerations.LogLevel.Info, $"Title: {plexRequest.Metadata.GrandparentTitle} | Episode: {plexRequest.Metadata.Index}\n", DateTime.Now);

            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted
            };

            return response;
        }

        //[HttpGet]
        //public string GetAllLogEntries()
        //{
        //    return "Hello World";
        //}
    }
}
