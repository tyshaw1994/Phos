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

namespace Phos.Controllers
{
    public class PlexController : ApiController
    {
        Logger logger = new Logger();

        [HttpPost]
        public HttpResponseMessage PostWebhook([FromBody] PlexRequest request)
        {
            if(!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            // TODO(Tyler): Figure out a way to utilize the other play events
            if(request.Event.Equals("media.scrobble"))
            {
                var malResponse = MyAnimeListManager.SearchForShow(request.Metadata.Title);
            }

            logger.CreateLogEntry(Enumerations.LogLevel.Info, request.ToString(), DateTimeOffset.UtcNow);

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
