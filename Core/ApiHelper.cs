using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Core
{
    public static class ApiHelper
    {
        public const string PostStatsURL = "api/ServerStats/PostStats";
        public const string PostDriveStatsURL = "api/ServerDisk/PostDriveStats";
        public const string PostServerURL = "api/Server/PostServer";

        private static HttpClient Client = new HttpClient();

        public static async Task APIPost(Object obj, string URI)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //WriteErrorLog("Posting stats to: " + cons.BaseAddress + URI);
            try
            {
                HttpResponseMessage msg = await Client.PostAsJsonAsync(new Uri("http://localhost:62104/" + URI), obj);
                //WriteErrorLog("Posted stats: " + msg.StatusCode);
            }
            catch (Exception e)
            {
                //WriteErrorLog(e);
            }

        }
    }
}
