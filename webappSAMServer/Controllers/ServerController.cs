using System.Web.Http;
using Core;
using webappSAMServer.Repositories;

namespace webappSAMServer.Controllers
{
    public class ServerController : ApiController
    {
        [HttpPost]
        public void PostServerHeader(BaseServer server)
        {
            //ServerRepository repository = new ServerRepository();
            new ServerRepository().PostServerHeader(server);    
        }

      
    }
}
