using System;
using System.Windows.Forms;
using Facade;
using System.Threading;

namespace TstatMgmtGUI
{
    public partial class CurrentNetwork : Form
    {        
        TstatPairing pair;
        private static int progress;       

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
            
            progress = 0;
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
            try
            {
                pair.ConnectToThermoStat(pair.ThermostatSSID);
                Thread.Sleep(5000);
                label1.Invoke((MethodInvoker)(() => label1.Text = "Successfully connected to " + pair.ThermostatSSID));
                btnContinue.Invoke((MethodInvoker)(() => btnContinue.Show()));
            }
            catch (Exception ex)
            {
                label1.Invoke((MethodInvoker)(() => label1.Text = ex.Message));
            }
        }
      

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.Hide();
            ShowVisibleNetworks svn = new ShowVisibleNetworks(pair);            
            svn.Show();
        }       
    }
}
