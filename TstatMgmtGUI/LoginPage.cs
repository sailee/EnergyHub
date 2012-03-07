using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TstatMgmtGUI
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("WARNING \nThis process will interupt your internet connection. Please save your open files before continuing.", "Warning", System.Windows.Forms.MessageBoxButtons.OKCancel);
            System.Windows.Forms.MessageBox.Show("UPDATING FIRMWARE \nWe found a newer version of the application. This will only take a minute.", "Updating Firmware");
            this.Hide();
            FindAvailableNetworks FindNetworks = new FindAvailableNetworks();
            FindNetworks.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://myhub.energyhub.net/EDX/public/create_account.html");
        }

        private void LoginPage_Load_1(object sender, EventArgs e)
        {

        }

    }
}
