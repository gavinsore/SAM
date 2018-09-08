using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Core;
using System.Diagnostics;


namespace svcSAMCollector
{
    public static class Library
    {
        public static void GetStats(BaseServer svr, PerformanceCounter cpuCounter, PerformanceCounter ramCounter)
        {
            svr.GetStats(cpuCounter, ramCounter);
            Core.ApiHelper.APIPost(svr, ApiHelper.PostStatsURL).Wait();
        }


        public static void GetDiskStats(BaseServer svr)
        {
            svr.GetDriveStats();
            Core.ApiHelper.APIPost(svr.Disks, ApiHelper.PostDriveStatsURL).Wait();
        }

        public static void WriteErrorLog(Exception ex)
        {
            WriteErrorLog(ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
        }

        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logfile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch { }

        }
    }
}