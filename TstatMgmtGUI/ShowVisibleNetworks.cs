using System;
using System.Windows.Forms;
using InternalDataTypes;
using System.Collections.Generic;
using Facade;
using System.Threading;


namespace TstatMgmtGUI
{
    public partial class ShowVisibleNetworks : Form
    {
        Boolean status;
        TstatPairing pair;        
             
        public ShowVisibleNetworks(TstatPairing pair)
        {
            this.pair = pair;
            InitializeComponent();
        }

        private void ShowVisibleNetworks_Load(object sender, EventArgs e)
        {
            try
            {               
                label1.Text = "Following is the list of networks accessible through your thermostat:";

                List<JsonNetwork> nets = pair.fetchNetworks();
                label1.Text = "List of visible networks";

                listBox1.DataSource = nets;
                listBox1.DisplayMember = "Ssid";              
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
        }       

        private void button1_Click(object sender, EventArgs e)
        {
            try{
            List<JsonNetwork> nets = pair.fetchNetworks();

            label1.Text = "List of visible networks";

            listBox1.DataSource = nets;
            listBox1.DisplayMember = "Ssid";
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {            
            pair.ThermostatSSID = listBox1.SelectedValue.ToString();

            try
            {
                Thread t = new Thread(UpdateStat);
                t.Start();

                while (!status)
                {
                    
                }


                label1.Text = "Successfully connected to " + pair.ThermostatSSID;             

            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
                label1.Text = "Could not connect to thermostat. Please try again.";
            }
        }

        private void UpdateStat()
        {
         //   status = pair.ConnectToNetwork(net);
        }
    }
}
