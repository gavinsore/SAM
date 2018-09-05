using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;

namespace RegisterSAM
{
    public partial class Form1 : Form
    {
        Guid ServerGuid;

        public Form1()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            ServerGuid = Guid.NewGuid();
            tbServerGuid.Text = ServerGuid.ToString();
        }

        private void LoadSettings()
        {
            ClientSettings cs = new ClientSettings();

            if (cs.LoadFromXML())
            {
                ServerGuid = cs.ServerGuid;
                tbServerGuid.Text = ServerGuid.ToString();

                tbCompanyName.Text = cs.CompanyName;
                tbSAMServerURL.Text = cs.SAMServerURL;
                tbStatsSeconds.Text = cs.StatsTick.ToString();
                tbDiskStatsSeconds.Text = cs.DiskStatsTick.ToString();
            }
            else
            {
                tbStatsSeconds.Text = "1";
                tbDiskStatsSeconds.Text = "60";
            }

        }

        private bool SaveSettings()
        {
            ClientSettings cs = new ClientSettings();

            if (!cs.SaveToXML(ServerGuid, tbCompanyName.Text, tbSAMServerURL.Text, int.Parse(tbStatsSeconds.Text), int.Parse(tbDiskStatsSeconds.Text)))
            {
                MessageBox.Show("Issue Saving Settings");
                return false;
            }
            else
            {
                MessageBox.Show("Save Successful");
                return true;
            }
                


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
