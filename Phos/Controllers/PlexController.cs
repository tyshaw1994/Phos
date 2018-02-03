using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Phos.Controllers
{
    public class PlexController : ApiController
    {
        [HttpPost]
        public void Post()
        {

        }

        [HttpGet]
        public string GetAllLogEntries()
        {
            return "Hello World";
        }
    }
}
