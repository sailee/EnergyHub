using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
//using Newtonsoft.Json;
using System.IO;

namespace TstatMgmtGUI
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://192.168.10.1/sys/scan");
            //HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();            
            
//            JsonTextReader reader = new JsonTextReader(rdr);
            //label1.Text = response.ToString();
        }
    }
}
