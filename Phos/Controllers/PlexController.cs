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
            PlexRequest plexRequest = new PlexRequest();
            try
            {
                plexRequest = JsonConvert.DeserializeObject<PlexRequest>(PlexManager.ParseJsonFromWebhook(content));
            }
            catch (JsonSerializationException jse)
            {
                Logger.CreateLogEntry(Enumerations.LogType.Error, jse, DateTime.Now);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            Logger.CreateLogEntry(Enumerations.LogType.Info, $"Incoming event ({plexRequest.Event}) from {plexRequest.Account.Title} for episode {plexRequest.Metadata.Index} of {plexRequest.Metadata.GrandparentTitle}", DateTime.Now);
            
            // TODO(Tyler): Figure out a way to utilize the other play events. Maybe Hue integration, email updates, some form of web ui, etc
            if (plexRequest.Event.Equals("media.scrobble"))
            {
                var show = MyAnimeListManager.SearchForShow(plexRequest.Metadata.GrandparentTitle);

                if (!(show is MalShow))
                {
                    Logger.CreateLogEntry(Enumerations.LogType.Error, new ArgumentException("Show was not found through Jikan API search or some other error occured."), DateTime.Now);
                }

                var id = show.Id;
                var episodeCompleted = plexRequest.Metadata.Index;
                var isFinished = (episodeCompleted == show.Episodes) ? true : false;

                // If I ever want to release this to the public, I will need some kind of lookup from a storage for MAL creds/emails, but for now I'll use my own
                if (plexRequest.Account.Title == "shaw.tyler94@gmail.com")
                {
                    var updated = MyAnimeListManager.UpdateList(id, episodeCompleted, isFinished);

                    if (!updated)
                    {
                        Logger.CreateLogEntry(Enumerations.LogType.Error, "Failed to update list with show.", DateTime.Now);
                    }
                }

                Logger.CreateLogEntry(Enumerations.LogType.Scrobble, $"Finished watching episode {episodeCompleted} of {plexRequest.Metadata.GrandparentTitle}", DateTime.Now);
            }

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
