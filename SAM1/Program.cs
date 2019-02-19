using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using Core;
using System.Net.Http.Formatting;

namespace SAM1
{
    class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            PerformanceCounter ramCounter = new PerformanceCounter
            {
                CategoryName = "Memory"
            };

            PerformanceCounter systemCounter = new PerformanceCounter
            {
                CategoryName = "System",
                CounterName = "Processor Queue Length"
            };

            ClientSettings cs = new ClientSettings();
            Core.BaseServer svr;
            if (cs.LoadFromXML())
            {
                svr = new BaseServer(cs.ServerGuid);
            }
            else
            {
                svr = new BaseServer(Guid.NewGuid());

            }
            Core.ApiHelper.APIPost(svr, ApiHelper.PostServerURL).Wait();

            for (int i = 0; i < 30; i++)
            {
                svr.GetStats(cpuCounter, ramCounter, systemCounter);

                Console.WriteLine("CPU Usage: " + svr.ProcessorTotal.ToString() + "%");
                Console.WriteLine("CPU Queue Length: " + svr.ProcessorQueueLength.ToString());
                Console.WriteLine("Memory in Use: " + svr.MemoryInUse.ToString() + "%");
                Console.WriteLine("Memory Available: " + svr.MemoryAvailable.ToString() + "Mb (" + (Math.Round(svr.MemoryAvailable/1024,1)) + "Gb)");

                Core.ApiHelper.APIPost(svr, ApiHelper.PostStatsURL).Wait();

                if (i % 5 == 0)
                {
                    svr.GetDriveStats();
                    foreach(BaseDisks disk in svr.Disks)
                    {
                        Console.WriteLine(disk.DiskLetter + " Total Size: " + disk.TotalDiskSize.ToString() + "Mb | Available Space: " + disk.FreeDiskSpace.ToString() + "Mb");
                        Console.WriteLine(disk.DiskLetter + " Read MBytes/sec: " + (disk.ReadBytesPerSec / 1048576).ToString("F") + " | Write Mbytes/sec: " + (disk.WriteBytesPerSec/1048576).ToString("F"));
                    }

                    Core.ApiHelper.APIPost(svr.Disks, ApiHelper.PostDriveStatsURL).Wait();
                }
                
                System.Threading.Thread.Sleep(1000);
            }
            
        }

    }
}
