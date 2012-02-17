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
using System.Diagnostics;

namespace TstatMgmtGUI
{
    public partial class FindAvailableNetworks : Form
    {
        TstatPairing pair;

        public FindAvailableNetworks()
        {
            InitializeComponent();
        }       

        private static bool StartsWithThermostat(Network n)
        {
            if (n.NetworkSSID.ToLower().StartsWith("thermostat-"))
            //if (n.NetworkSSID.ToLower().StartsWith("tran"))
                return true;
            else return false;
        }

        private void btnJoinNetwork_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (lbAvailableTstats.Items.Count > 0)
            {
                Network netw = (Network)lbAvailableTstats.SelectedItem;
                pair.ThermostatSSID = netw.NetworkSSID;
                Debug.WriteLine("You asked to join {0}", pair.ThermostatSSID);                
                
                CurrentNetwork cn = new CurrentNetwork(pair);
                cn.ShowDialog();
            }
            else 
            {
                Error err = new Error();
                err.ShowDialog();
            }
        }

        private void FindAvailableNetworks_Load(object sender, EventArgs e)
        {
            this.Show();
            pair = new TstatPairing();
            
            try
            {
                List<Network> networks = pair.GetAvailableThermostats().FindAll(StartsWithThermostat);

                if (networks.Count == 0)
                {
                    Debug.Write("Could not find any thermostats.");
                }

                //lbAvailableTstats.Items.Clear();
                lbAvailableTstats.DataSource = networks;
                lbAvailableTstats.DisplayMember = "NetworkSSID";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }    
}
