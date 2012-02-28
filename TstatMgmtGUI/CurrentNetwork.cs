using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facade;

namespace TstatMgmtGUI
{
    public partial class CurrentNetwork : Form
    {        
        TstatPairing pair;

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
            label1.Text= "'"+pair.ThermostatSSID + "' will be connected to '"+ pair.getConnectedNetwork()+"'.";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                pair.ConnectToThermoStat(pair.ThermostatSSID);
                label1.Text = "Successfully connected to " + pair.ThermostatSSID;
                btnOK.Hide();
                btnContinue.Show();
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            ShowVisibleNetworks svn = new ShowVisibleNetworks();
            svn.Show();
        }       
    }
}
