using System;
using System.Windows.Forms;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using InternalDataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;


namespace TstatMgmtGUI
{
    public partial class ShowVisibleNetworks : Form
    {
             
        public ShowVisibleNetworks()
        {
            InitializeComponent();
        }

        private void ShowVisibleNetworks_Load(object sender, EventArgs e)
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
                List<JsonNetwork> net =  JsonNetDeserialize(sb.ToString());

                label1.Text = "List of visible networks";

                listBox1.DataSource = net;
                listBox1.DisplayMember = "Ssid";                

                //JsonNets nets = (JsonNets)ser.Deserialize(sb.ToString(), typeof(JsonNets));

                
                //JsonNets list = new JsonNets();
                //JsonNetwork net = new JsonNetwork("Trantor", "bssid", 1, 2, 3);
                //list.networks.Add(net);

                //net = new JsonNetwork("Tsyay", "bssid1", 11, 12, 13);
                //list.networks.Add(net);

                //String str = ser.Serialize(list);
                //label1.Text = str;

                //JsonNets net = (JsonNets)ser.ReadObject(resStream);
               
                

                //int cnt = network.GetLength(0);

                //while (cnt > 0)
                //{
                //    net = network[cnt];
                //}
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
        }

        //private void DeSerialize(String json)
        //{
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonNets));
        //    JsonNets obj;

        //    using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
        //    {
        //        obj = ser.ReadObject(ms) as JsonNets;                
        //    }

        //    IEnumerator<JsonNetwork> itr = obj.networks.GetEnumerator();
        //    itr.MoveNext();

        //    while (!Object.Equals(itr.Current, null))
        //    {
        //        JsonNetwork net = itr.Current;
        //        itr.MoveNext();
        //    }
        //}

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
    }
}
