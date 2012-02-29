using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facade;
using InternalDataTypes;
using System.Threading;

namespace TstatMgmtGUI
{
    public partial class ConnectToHomeNetwork : Form
    {
        TstatPairing pair;
        JsonNetwork currentNetwork;

        public ConnectToHomeNetwork()
        {
            InitializeComponent();
        }

        public ConnectToHomeNetwork(JsonNetwork net, TstatPairing pair)
        {
            InitializeComponent();
            this.pair = pair;
            this.currentNetwork = net;
        }


        private void ConnectToHomeNetwork_Load(object sender, EventArgs e)
        {
            pair.ThermostatSSID = currentNetwork.Ssid;
            label1.Text = label1.Text + " " + currentNetwork.Ssid;            
        }

        private void Connect()
        {            
            pair.ConnectToNetwork(currentNetwork);
            Thread.Sleep(5000);
            label1.Invoke((MethodInvoker)(() => label1.Text ="Successfully connected to " + pair.ThermostatSSID));           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "Attempting to connect to " + pair.ThermostatSSID;
                button1.Hide();
                txtPassword.Hide();
                Thread t = new Thread(Connect);
                t.Start();              
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
                label1.Text = "Could not connect to thermostat. Please try again.";
            }
        }
    }
}
