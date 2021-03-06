﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.IO;
using System.Net.Http;
using System.Timers;

namespace svcSAMCollector
{
    public partial class SAMCollector : ServiceBase
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
       // private HttpClient cons;
        private BaseServer svr;
        private Timer timer1;
        private Timer timerDisk;

        public SAMCollector()
        {
            InitializeComponent();
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter();
            ramCounter.CategoryName = "Memory";

            /*cons = new HttpClient();
            cons.BaseAddress = new Uri("http://localhost:62104/");
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));*/

            
            //svr.ServerID = Guid.NewGuid();
            //svr.ServerName = System.Environment.MachineName;
            //svr.OS = System.Environment.OSVersion.ToString();

            
        }

        protected override void OnStart(string[] args)
        {
            ClientSettings cs = new ClientSettings();
            timer1 = new Timer();
            timerDisk = new Timer();
            try
            {
                if (cs.LoadFromXML())
                {
                    svr = new BaseServer(cs.ServerGuid);
                   // svr.ServerID = cs.ServerGuid;
                    //tbServerGuid.Text = ServerGuid.ToString();

                    //tbCompanyName.Text = cs.CompanyName;
                    //tbSAMServerURL.Text = cs.SAMServerURL;

                    timer1.Interval = cs.StatsTick * 1000;
                    timerDisk.Interval = cs.DiskStatsTick * 1000;
                    this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
                    this.timerDisk.Elapsed += new System.Timers.ElapsedEventHandler(this.timerDisk_Tick);
                    timer1.Enabled = true;
                    timerDisk.Enabled = true;

                    Library.WriteErrorLog("Collecting server statistics service");
                    Library.WriteErrorLog("Stats Tick: " + timer1.Interval.ToString());
                    Library.WriteErrorLog("Disk Stats Tick: " + timerDisk.Interval.ToString());

                }
                else
                {
                    Library.WriteErrorLog(AppDomain.CurrentDomain.BaseDirectory);
                    Library.WriteErrorLog("Server has not been registered.  Stopping Service");
                    this.Stop();
                }
            }
            catch (Exception ex)
            {
                Library.WriteErrorLog(ex);
            }
        }

        protected override void OnStop()
        {
            timer1.Stop();
            timerDisk.Stop();
            cpuCounter.Dispose();
            ramCounter.Dispose();
           // cons.Dispose();
            Library.WriteErrorLog("Stopping server statistics service");
        }

        protected override void OnPause()
        {
            timer1.Stop();
            timerDisk.Stop();
            Library.WriteErrorLog("Pausing server statistics service");
            base.OnPause();

        }

        protected override void OnContinue()
        {
            timer1.Start();
            timerDisk.Start();
            Library.WriteErrorLog("Resuming server statistics service");
            base.OnContinue();
        }

        private void timer1_Tick(Object sender, ElapsedEventArgs e)
        {
            Library.GetStats(svr, cpuCounter, ramCounter);

        }

        private void timerDisk_Tick(Object sender, ElapsedEventArgs e)
        {
            Library.GetDiskStats(svr);
        }
    }
}
