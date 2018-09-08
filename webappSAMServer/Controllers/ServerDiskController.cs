using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Core;
using webappSAMServer.Repositories;

namespace webappSAMServer.Controllers
{
    public class ServerDiskController : ApiController
    {
        [HttpPost]
        public void PostDiskStats(List<BaseDisks> disks)
        {
            new ServerRepository().PostDiskStats(disks);
        }
    }
}