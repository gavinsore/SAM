using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

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
        public double MemoryInUse { get; set; }
        public double MemoryAvailable { get; set; }
        public double TotalMemory { get; private set; }
        public string OS { get; set; }

        public BaseServer()
        {
            Disks = new List<BaseDisks>();
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
    }
}
