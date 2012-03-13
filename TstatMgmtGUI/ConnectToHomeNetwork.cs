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
            this.pair = new TstatPairing();
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
            txtNetworkName.Text= currentNetwork.Ssid;   
            //Fetch Locations
        }

        private void Connect()
        {
            //configure home network onto the Thermostat
            Boolean result = pair.ConfigureHomeNetwork(currentNetwork, txtPIN.Text, txtPassword.Text );
            //if connection is successful, connect back to the home network, via 
            if (result)
            {
                pair.ConnectToNetwork(currentNetwork);
                this.Hide();
                ServiceDiscovery sd = new ServiceDiscovery();
                sd.Show();
            }
        }       

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {                
                //label1.Text = "Attempting to connect to " + pair.ThermostatSSID;
                //btnConnect.Hide();
                //txtPassword.Hide();
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
