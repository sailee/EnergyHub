using System;
using System.Windows.Forms;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using InternalDataTypes;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Facade;
using System.Threading;


namespace TstatMgmtGUI
{
    public partial class ShowVisibleNetworks : Form
    {
        Boolean status;
        TstatPairing pair;
        CurrentNetwork cn;
             
        public ShowVisibleNetworks(CurrentNetwork cn)
        {
            this.cn = cn;
            InitializeComponent();
        }

        private void ShowVisibleNetworks_Load(object sender, EventArgs e)
        {
            pair = new TstatPairing();
            label1.Text = "Following is the list of networks accessible through your thermostat:";
            
            fetchNetworks();
            cn.Hide();
        }

        private List<JsonNetwork> JsonNetDeserialize(string json)
        {
            JObject jsonObj = JObject.Parse(json);

            JArray networks = (JArray)jsonObj["networks"];

            List<JsonNetwork> nets = new List<JsonNetwork>();

            foreach (JToken token in networks)
            {
               String ssid = (String)token[0];
               String bssid = (String)token[1];
               int secMode = (int)token[2];
               int Channel = (int)token[3];
               int rssi = (int)token[4];

               JsonNetwork net = new JsonNetwork(ssid, bssid, secMode, Channel, rssi);
               nets.Add(net);
            }

            return nets;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fetchNetworks();
        }

        private void fetchNetworks()
        {
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://192.168.10.1/sys/scan");
                HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                Stream resStream = response.GetResponseStream();

                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonNets));
                JavaScriptSerializer ser = new JavaScriptSerializer();

                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                }
                while (count > 0);

                //DeSerialize(sb.ToString());
                List<JsonNetwork> net = JsonNetDeserialize(sb.ToString());

                label1.Text = "List of visible networks";

                listBox1.DataSource = net;
                listBox1.DisplayMember = "Ssid";

            }
            catch (Exception)
            {
                label1.Text = "Could not fetch the network list from your thermostat.";
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
