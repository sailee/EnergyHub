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

        public CurrentNetwork()
        {
            InitializeComponent();
        }

        public CurrentNetwork(TstatPairing obj)
        {
            InitializeComponent();
            pair = obj;
            
        }

        private void CurrentNetwork_Load(object sender, EventArgs e)
        {
            
            btnContinue.Hide();
            //label1.Text= "'"+pair.ThermostatSSID + "' will be connected to '"+ pair.getConnectedNetwork()+"'.";

            try
            {
                Thread t = new Thread(UpdateStat);
                t.Start();
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
            label1.Invoke((MethodInvoker)(() => label1.Text = "Successfully connected to " + pair.ThermostatSSID));
            btnContinue.Invoke((MethodInvoker)(() => btnContinue.Show()));     
        }
      

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.Hide();
            ShowVisibleNetworks svn = new ShowVisibleNetworks(pair);            
            svn.Show();
        }       
    }
}
