using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.IO;

namespace Core
{
    public class BaseDisks
    {
        public string DiskLetter { get; set; }
        public long TotalDiskSize { get; set; }
        public long FreeDiskSpace { get; set; }
        public Guid ServerID { get; }

        public float ReadBytesPerSec { get; set; }
        public float WriteBytesPerSec { get; set; }

        public BaseDisks(Guid p_ServerID)
        {
            ServerID = p_ServerID;
        }
    }

    public class BaseServer
    {
        public Guid ServerID { get; set;  }
        public string ServerName { get; set; }
        public DateTime PostTime { get; private set; }
        public double ProcessorTotal { get; set; }

        public double ProcessorQueueLength { get; set; }
        public List<BaseDisks> Disks { get; set; }
        public double MemoryInUse { get { return GetMemoryInUse(); } }
        public double MemoryAvailable { get; set; }
        public double TotalMemory { get; set; }
        public string OS { get; set; }
        public string ServicePack { get; set; }

        public TimeSpan UpTime
        {
            get
            {
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();       //Call this an extra time before reading its value
                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
            }
        }

        public DateTime BootTime
        {
            get { return DateTime.Now.Subtract(UpTime); }
        }

        public BaseServer(Guid p_serverid)
        {
            Disks = new List<BaseDisks>();
            ServerName = System.Environment.MachineName;
            OS = System.Environment.OSVersion.Version.ToString();
            ServicePack = System.Environment.OSVersion.ServicePack;
            ServerID = p_serverid;
            GetTotalMemory();
        }

        private double GetMemoryInUse()
        {
            return Math.Round(((this.TotalMemory - this.MemoryAvailable) / this.TotalMemory) * 100, 1);
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

        public void GetStats(PerformanceCounter cpuCounter, PerformanceCounter ramCounter, PerformanceCounter systemCounter)
        {
            cpuCounter.NextValue();
            systemCounter.NextValue();
            System.Threading.Thread.Sleep(500);  // now matches task manager reading

            this.ProcessorTotal = Math.Round(cpuCounter.NextValue(), 1);
            this.ProcessorQueueLength = systemCounter.NextValue();

            ramCounter.CounterName = "Available MBytes";
            this.MemoryAvailable = Math.Round(ramCounter.NextValue(), 1);
        }

        public void GetDriveStats()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            var cat = new System.Diagnostics.PerformanceCounterCategory("PhysicalDisk");
            var instNames = cat.GetInstanceNames();

            PerformanceCounter diskCounter = new PerformanceCounter
            {
                CategoryName = "PhysicalDisk" 
            };

            foreach (DriveInfo drive in drives)
            {
                //There are more attributes you can use.
                //Check the MSDN link for a complete example.

                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                {
                    BaseDisks disk = null;
                    if (this.Disks != null)
                        disk = this.Disks.Find(x => x.DiskLetter == drive.Name);

                    if (disk == null)
                    {
                        disk = new BaseDisks(this.ServerID);
                        this.Disks.Add(disk);
                    }

                    disk.DiskLetter = drive.Name;
                    disk.FreeDiskSpace = ((drive.AvailableFreeSpace / 1024) / 1024);
                    disk.TotalDiskSize = ((drive.TotalSize / 1024) / 1024);

                    diskCounter.CounterName = "Disk Read Bytes/sec";
                    diskCounter.InstanceName = instNames.Where(x => x.Contains(drive.Name[0])).First();
                    diskCounter.NextValue();
                    System.Threading.Thread.Sleep(500);
                    disk.ReadBytesPerSec = diskCounter.NextValue();

                    diskCounter.CounterName = "Disk Write Bytes/sec";
                    diskCounter.InstanceName = instNames.Where(x => x.Contains(drive.Name[0])).First();
                    diskCounter.NextValue();
                    System.Threading.Thread.Sleep(500);
                    disk.WriteBytesPerSec = diskCounter.NextValue();
                }
            }


        }

    }
}
