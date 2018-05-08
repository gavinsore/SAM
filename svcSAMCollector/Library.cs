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
        static async Task MyAPIPost(HttpClient cons, BaseServer server)
        {
            WriteErrorLog("Posting stats to: " + cons.BaseAddress);
            WriteErrorLog("CPU: " + server.ProcessorTotal.ToString());
            try
            {
                HttpResponseMessage msg = await cons.PostAsJsonAsync("api/Server/PostStats", server);
                WriteErrorLog("Posted stats");
            }
            catch (Exception e)
            {
                WriteErrorLog(e);
            }
            
        }

        public static void GetStats(BaseServer svr, PerformanceCounter cpuCounter, PerformanceCounter ramCounter, DriveInfo[] drives)
        {
            dynamic firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(500);
            // now matches task manager reading
            svr.ProcessorTotal = Math.Round(cpuCounter.NextValue(), 1);

            ramCounter.CounterName = "% Committed Bytes In Use";
            svr.MemoryInUse = Math.Round(ramCounter.NextValue());

            ramCounter.CounterName = "Available MBytes";
            svr.MemoryAvailable = Math.Round(ramCounter.NextValue() / 1024, 1);

            drives = DriveInfo.GetDrives();
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
                cons.BaseAddress = new Uri("http://localhost:62104/");
                cons.DefaultRequestHeaders.Accept.Clear();
               // cons.Timeout = new TimeSpan(500);
                cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                MyAPIPost(cons, svr).Wait();
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