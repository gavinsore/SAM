using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;

namespace Core
{
    public class BaseDisks
    {
        public string DiskLetter { get; set; }
        public long TotalDiskSize { get; set; }
        public long FreeDiskSpace { get; set; }
    }

    public class BaseServer
    {
        public Guid ServerID { get; set; }
        public string ServerName { get; set; }
        public DateTime PostTime { get; set; }
        public double ProcessorTotal { get; set; }
        public List<BaseDisks> Disks { get; set; }
        public double MemoryInUse { get { return GetMemoryInUse(); } }
        public double MemoryAvailable { get; set; }
        public double TotalMemory { get; private set; }
        public string OS { get; set; }

        public BaseServer()
        {
            Disks = new List<BaseDisks>();
            GetTotalMemory();
        }

        private double GetMemoryInUse()
        {
            return ((this.TotalMemory - this.MemoryAvailable) / this.TotalMemory) * 100;
        }

        public void GetTotalMemory()
        {
            string Query = "SELECT * FROM Win32_ComputerSystem";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);
            foreach (ManagementObject moMem in searcher.Get())
            {
                this.TotalMemory = Convert.ToDouble(moMem.Properties["TotalPhysicalMemory"].Value) / 1024 / 1024;  //from KB to MB
            }

        }

        public void GetStats(PerformanceCounter cpuCounter, PerformanceCounter ramCounter)
        {
            dynamic firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(500);  // now matches task manager reading

            this.ProcessorTotal = Math.Round(cpuCounter.NextValue(), 1);

            ramCounter.CounterName = "Available MBytes";
            this.MemoryAvailable = Math.Round(ramCounter.NextValue(), 1);
        }
    }
}
