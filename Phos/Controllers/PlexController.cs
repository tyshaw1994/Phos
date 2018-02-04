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
        Logger logger = new Logger();

        [HttpPost]
        public async Task<HttpResponseMessage> PostWebhook()
        {
            //if(!ModelState.IsValid)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            //}

            //// TODO(Tyler): Figure out a way to utilize the other play events
            //if(request.Event.Equals("media.scrobble"))
            //{
            //    var malResponse = MyAnimeListManager.SearchForShow(request.Metadata.Title);
            //}

            //logger.CreateLogEntry(Enumerations.LogLevel.Info, request.ToString(), DateTimeOffset.UtcNow);
            var content = await this.Request.Content.ReadAsStringAsync();

            // try to parse the json out of the string
            var json = PlexManager.ParseJsonFromWebhook(content);
            logger.CreateLogEntry(Enumerations.LogLevel.Info, json, DateTimeOffset.UtcNow);

            var testJson = JsonConvert.DeserializeObject<PlexRequest>(json);
            logger.CreateLogEntry(Enumerations.LogLevel.Info, testJson.Metadata.Title, DateTimeOffset.UtcNow);

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
