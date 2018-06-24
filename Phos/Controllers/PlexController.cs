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
using Phos.Models.AniList;

namespace Phos.Controllers
{
    public class PlexController : ApiController
    {
        private RegisterValues values;

        [HttpPost]
        [Route("Plex/PostWebhook")]
        public async Task<PhosResponse> PostWebhook()
        {
            if (!ModelState.IsValid)
            {
                return new PhosResponse { MalResult = false, AniListResult = false };
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
                return new PhosResponse { MalResult = false, AniListResult = false };
            }

            Logger.CreateLogEntry(Enumerations.LogType.Info, $"Incoming event ({plexRequest.Event}) from {plexRequest.Account.Title} for episode {plexRequest.Metadata.Index} of {plexRequest.Metadata.GrandparentTitle}", DateTime.Now);

            values = RegisterValues.GetRegisteredValues();

            bool malResult = false, aniListResult = false;
            if (plexRequest.Event.Equals("media.scrobble"))
            {
                // MAL
                malResult = MyAnimeListManager.UpdateList(values, plexRequest);

                // AniList
                aniListResult = await AniListManager.UpdateListAsync(values, plexRequest);

                Logger.CreateLogEntry(Enumerations.LogType.Scrobble, $"Finished watching episode {plexRequest.Metadata.Index} of {plexRequest.Metadata.GrandparentTitle}", DateTime.Now);
            }

            return new PhosResponse { MalResult = malResult, AniListResult = aniListResult };
        }

        [HttpGet]
        [Route("GetAniList")]
        public async Task<bool> GetAniListToken()
        {
            values = RegisterValues.GetRegisteredValues();
            return await AniListManager.UpdateListAsync(values, new PlexRequest());
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