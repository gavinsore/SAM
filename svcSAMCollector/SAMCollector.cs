using System;
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
        private DriveInfo[] drives;
       // private HttpClient cons;
        private BaseServer svr;
        private Timer timer1; 

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

            svr = new BaseServer();
            svr.ServerID = Guid.NewGuid();
            svr.ServerName = System.Environment.MachineName;
            svr.OS = System.Environment.OSVersion.ToString();

            
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer(10000);
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;

            Library.WriteErrorLog("Collecting server statistics service");

        }

        protected override void OnStop()
        {
            timer1.Stop();
            cpuCounter.Dispose();
            ramCounter.Dispose();
           // cons.Dispose();
            Library.WriteErrorLog("Stopping server statistics service");
        }

        protected override void OnPause()
        {
            timer1.Stop();
            Library.WriteErrorLog("Pausing server statistics service");
            base.OnPause();

        }

        protected override void OnContinue()
        {
            timer1.Start();
            Library.WriteErrorLog("Resuming server statistics service");
            base.OnContinue();
        }

        private void timer1_Tick(Object sender, ElapsedEventArgs e)
        {
            Library.GetStats(svr, cpuCounter, ramCounter, drives);

        }
    }
}
