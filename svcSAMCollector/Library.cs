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
        const string PostStatsURL = "api/Server/PostStats";
        const string PostDriveStatsURL = "api/Server/PostDriveStats";

        static async Task MyAPIPost(HttpClient cons, BaseServer server, string URI)
        {
            WriteErrorLog("Posting stats to: " + cons.BaseAddress + URI);
            try
            {
                HttpResponseMessage msg = await cons.PostAsJsonAsync(URI, server);
                WriteErrorLog("Posted stats: " + msg.StatusCode);
            }
            catch (Exception e)
            {
                WriteErrorLog(e);
            }

        }

        /*public static void GetTotalMemory(BaseServer svr)
        {
            string Query = "SELECT * FROM Win32_LogicalMemoryConfiguration";
            double SizeinMB = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);
            foreach (ManagementObject moMem in searcher.Get())
            {
                SizeinMB = Convert.ToDouble(moMem.Properties["TotalPhysicalMemory"].Value) / 1024;  //from KB to MB
            }

            svr.TotalMemory = SizeinMB;
        }*/

        public static void GetStats(BaseServer svr, PerformanceCounter cpuCounter, PerformanceCounter ramCounter)
        {
            dynamic firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(500);
            // now matches task manager reading
            svr.ProcessorTotal = Math.Round(cpuCounter.NextValue(), 1);

            ramCounter.CounterName = "Available MBytes";
            svr.MemoryAvailable = Math.Round(ramCounter.NextValue(), 1);

            //ramCounter.CounterName = "% Committed Bytes In Use";
            if (svr.TotalMemory == 0)
                svr.GetTotalMemory();

            if (svr.TotalMemory > 0)
                svr.MemoryInUse = ((svr.TotalMemory - svr.MemoryAvailable) / svr.TotalMemory) * 100;

            using (HttpClient cons = new HttpClient())
            {
                cons.BaseAddress = new Uri("http://localhost:62104/");
                cons.DefaultRequestHeaders.Accept.Clear();
                cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //ConfigHttpClient(cons);

                MyAPIPost(cons, svr, PostStatsURL).Wait();
            }
        }

        private static void ConfigHttpClient(HttpClient p_httpclient)
        {
            p_httpclient.BaseAddress = new Uri("http://localhost:62104/");
            p_httpclient.DefaultRequestHeaders.Accept.Clear();
            p_httpclient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }


        public static void GetDiskStats(BaseServer svr)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                //There are more attributes you can use.
                //Check the MSDN link for a complete example.

                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                {
                    //Console.WriteLine(drive.Name + "Total Size: " + ((drive.TotalSize / 1024) / 1024) / 1024 + "Gb | Available Space: " + ((drive.AvailableFreeSpace / 1024) / 1024) / 1024 + "Gb");

                }
            }

            using (HttpClient cons = new HttpClient())
            {
                ConfigHttpClient(cons);

                MyAPIPost(cons, svr, PostDriveStatsURL).Wait();
            }

        }

        public static void WriteErrorLog(Exception ex)
        {
            /* StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logfile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch { }*/

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