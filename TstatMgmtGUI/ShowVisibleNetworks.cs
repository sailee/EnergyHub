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
                label1.Text = "Attempting to fetch the list of networks \n accessible through your thermostat.";
                listBox1.Hide();
                button1.Hide();
                button2.Hide();

                Thread t = new Thread(threadMethod);
                t.Start();
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
        }  
     
        private void threadMethod()
        {
            try
            {
                List<JsonNetwork> nets = pair.fetchNetworks();

                label1.Invoke((MethodInvoker)(() => label1.Text = "Following is the list of networks \n accessible through your thermostat:"));
                listBox1.Invoke((MethodInvoker)(() => listBox1.DataSource = nets));
                listBox1.Invoke((MethodInvoker)(() => listBox1.DisplayMember = "Ssid"));
                listBox1.Invoke((MethodInvoker)(() => listBox1.Visible = true));
                button2.Invoke((MethodInvoker)(() => button2.Show()));                
            }
            catch (Exception ex)
            {
                label1.Invoke((MethodInvoker)(() => label1.Text = ex.Message));
                button1.Invoke((MethodInvoker)(() => button1.Show()));
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

            ConnectToHomeNetwork cth = new ConnectToHomeNetwork((JsonNetwork)listBox1.SelectedItem, pair);
            this.Hide();
            cth.Show();           
        }

        private void UpdateStat()
        {
         //   status = pair.ConnectToNetwork(net);
        }
    }
}
