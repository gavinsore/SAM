﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using Core;

namespace SAM1
{
    class Program
    {
        static async Task MyAPIPost(HttpClient cons, BaseServer server)
        {
            await cons.PostAsJsonAsync("api/Server/PostStats", server);
        }

        static void Main(string[] args)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            dynamic firstValue;

            PerformanceCounter ramCounter = new PerformanceCounter();
            ramCounter.CategoryName = "Memory";

            DriveInfo[] drives;

            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri("http://localhost:62104/");
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Core.BaseServer svr = new BaseServer();
            svr.ServerID = Guid.NewGuid();
            svr.ServerName = System.Environment.MachineName;
            svr.OS = System.Environment.OSVersion.ToString();

            for (int i = 0; i < 30; i++)
            {

                firstValue = cpuCounter.NextValue();
                System.Threading.Thread.Sleep(500);
                // now matches task manager reading
                svr.ProcessorTotal = Math.Round(cpuCounter.NextValue(), 1);
                Console.WriteLine("CPU Usage: " + svr.ProcessorTotal.ToString() + "%");

                ramCounter.CounterName = "% Committed Bytes In Use";
                svr.MemoryInUse = Math.Round(ramCounter.NextValue());
                Console.WriteLine("Memory in Use: " + svr.MemoryInUse.ToString() + "%");

                ramCounter.CounterName = "Available MBytes";
                svr.MemoryAvailable = Math.Round(ramCounter.NextValue() / 1024, 1);
                Console.WriteLine("Memory Available: " + svr.MemoryAvailable.ToString() + "Gb");

                drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                        //There are more attributes you can use.
                        //Check the MSDN link for a complete example.

                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    {
                        Console.WriteLine(drive.Name + "Total Size: " + ((drive.TotalSize / 1024) / 1024) / 1024 + "Gb | Available Space: " + ((drive.AvailableFreeSpace / 1024) / 1024) / 1024 + "Gb");

                    }
                }

                MyAPIPost(cons, svr).Wait();
                System.Threading.Thread.Sleep(1000);
            }
            
        }

    }
}
