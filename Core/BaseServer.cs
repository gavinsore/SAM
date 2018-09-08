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

        public BaseDisks(Guid p_ServerID)
        {
            ServerID = p_ServerID;
        }
    }

    public class BaseServer
    {
        public Guid ServerID { get; }
        public string ServerName { get; private set; }
        public DateTime PostTime { get; private set; }
        public double ProcessorTotal { get; private set; }
        public List<BaseDisks> Disks { get; private set; }
        public double MemoryInUse { get { return GetMemoryInUse(); } }
        public double MemoryAvailable { get; private set; }
        public double TotalMemory { get; private set; }
        public string OS { get; private set; }
        public string ServicePack { get; private set; }

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

        public void GetStats(PerformanceCounter cpuCounter, PerformanceCounter ramCounter)
        {
            dynamic firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(500);  // now matches task manager reading

            this.ProcessorTotal = Math.Round(cpuCounter.NextValue(), 1);

            ramCounter.CounterName = "Available MBytes";
            this.MemoryAvailable = Math.Round(ramCounter.NextValue(), 1);
        }

        public void GetDriveStats()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
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
                }
            }


        }
    }
}
