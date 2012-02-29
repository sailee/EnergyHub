using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facade;
using System.Threading;

namespace TstatMgmtGUI
{
    public partial class CurrentNetwork : Form
    {        
        TstatPairing pair;
        private static string str;
        Boolean status=false;
        FindAvailableNetworks fan;

        public CurrentNetwork()
        {
            InitializeComponent();
        }

        public CurrentNetwork(TstatPairing obj, FindAvailableNetworks net)
        {
            InitializeComponent();
            pair = obj;
            fan = net;
        }

        private void CurrentNetwork_Load(object sender, EventArgs e)
        {
            
            btnContinue.Hide();
            //label1.Text= "'"+pair.ThermostatSSID + "' will be connected to '"+ pair.getConnectedNetwork()+"'.";

            try
            {
                Thread t = new Thread(UpdateStat);
                t.Start();

                while (!status)
                {
                    // label2.Text = "Status: " + pair.Status + " Reason Code: " + pair.ReasonCode;
                    //label1.Text = "Could not connect to Thermostat";
                    //btnOK.Text = "Try Again";
                    //btnOK.Show();
                    //btnContinue.Hide();
                }
                fan.Hide();

                label1.Text = "Successfully connected to " + pair.ThermostatSSID;
        
                btnContinue.Show();

            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
                label1.Text = "Could not connect to thermostat. Please try again.";
            }
        }
        
        private void UpdateStat()
        {
            status = pair.ConnectToThermoStat(pair.ThermostatSSID);
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            ShowVisibleNetworks svn = new ShowVisibleNetworks(this);            
            svn.Show();
        }       
    }
}
