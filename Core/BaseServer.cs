using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BaseDisks
    {
        public string DiskLetter { get; set; }
        public float TotalDiskSize { get; set; }
        public float FreeDiskSpace { get; set; }
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
        public string OS { get; set; }
    }
}
