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
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Phos.Controllers
{
    public class PlexController : ApiController
    {
        [HttpPost]
        [Route("Plex/PostWebhook")]
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

            RegisterValues values = MyAnimeListManager.GetRegisteredValues();

            // TODO(Tyler): Figure out a way to utilize the other play events. Maybe Hue integration, email updates, some form of web ui, etc
            if (plexRequest.Event.Equals("media.scrobble"))
            {
                Anime show = MyAnimeListManager.SearchListForShow(values.UserName, plexRequest.Metadata.GrandparentTitle);

                if (!(show is Anime) || string.IsNullOrEmpty(show.Title))
                {
                    Logger.CreateLogEntry(Enumerations.LogType.Error, new ArgumentException("Show was not found through MAL API search or some other error occured."), DateTime.Now);
                }

                var episodeCompleted = plexRequest.Metadata.Index;
                var isFinished = (episodeCompleted == show.Episodes) ? true : false;

                // If I ever want to release this to the public, I will need some kind of lookup from a storage for MAL creds/emails, but for now I'll use my own
                if (plexRequest.Account.Title == values.Email)
                {
                    var updated = MyAnimeListManager.UpdateList(values.UserName, values.Password, show, episodeCompleted, isFinished);

                    if (!updated)
                    {
                        Logger.CreateLogEntry(Enumerations.LogType.Error, "Failed to update list with show.", DateTime.Now);
                    }
                    else
                    {
                        TwilioClient.Init("ACb37004370a29383ed11dcf49a74924e2", "ec30b8521502b2c682482fe6045f73f7");
                        MessageResource.Create(
                            to: new Twilio.Types.PhoneNumber("+16037068203"),
                            from: new Twilio.Types.PhoneNumber("+19789653287"),
                            body: $"Successfully updated MAL with episode {episodeCompleted} of {show.Title}");
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

        [HttpPost]
        [Route("Plex/Register")]
        public async Task<HttpResponseMessage> Register()
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var content = await this.Request.Content.ReadAsStringAsync();
            RegisterValues values = new RegisterValues();

            try
            {
                values = JsonConvert.DeserializeObject<RegisterValues>(content);
            }
            catch (JsonSerializationException jse)
            {
                Logger.CreateLogEntry(Enumerations.LogType.Error, jse, DateTime.Now);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (!MyAnimeListManager.RegisterCredentials(values))
            {
                Logger.CreateLogEntry(Enumerations.LogType.Error, "Failed to register creds", DateTime.Now);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}