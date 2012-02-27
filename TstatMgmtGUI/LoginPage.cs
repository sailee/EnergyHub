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
            this.Hide();
            FindAvailableNetworks FindNetworks = new FindAvailableNetworks();
            FindNetworks.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://myhub.energyhub.net/EDX/public/create_account.html");
        }
    }
}
