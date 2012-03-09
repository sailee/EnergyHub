using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Facade;

namespace TstatMgmtGUI
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
         }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage Welcome = new LoginPage();
            Welcome.ShowDialog();            
        }

    }
}
