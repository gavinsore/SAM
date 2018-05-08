using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core;
using webappSAMServer.Repositories;

namespace webappSAMServer.Controllers
{
    public class ServerController : ApiController
    {
        [HttpPost]
        public void PostStats(BaseServer serverstats)
        {
            //ServerRepository repository = new ServerRepository();
            new ServerRepository().PostStats(serverstats);    
        }
    //return "Success";
    }
}
