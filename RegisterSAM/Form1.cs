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

            }

        }

        private bool SaveSettings()
        {
            ClientSettings cs = new ClientSettings();

            if (!cs.SaveToXML(ServerGuid, tbCompanyName.Text, tbSAMServerURL.Text))
            {

                MessageBox.Show("Issue Saving Settings");
                return false;
            }
            else
                return true;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
