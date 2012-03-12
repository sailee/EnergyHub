using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NativeWifi;
using InternalDataTypes;
using System.Collections;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace Facade
{
    public class TstatPairing
    {
        private WlanClient client;
        private string thermostatSSID;
        private object var,src;

        public String Status
        {
            get { 
                if(var!=null) 
                    return var.ToString();
                return "Initializing";
            }
        }

        public String ReasonCode
        {
            get { 
                if(src != null)
                    return src.ToString();
                return "";
            }
        }

        public string ThermostatSSID
        {
            get { return thermostatSSID; }
            set { thermostatSSID = value; }
        }
        
        public TstatPairing()
        {
            client = new WlanClient();
            thermostatSSID = string.Empty;         
        }
        
        public List<Network> GetAvailableThermostats()
        {
            List<Network> lstNetworks = new List<Network>();
            
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {   
                    lstNetworks.Add(new Network(network));                 
                }
            }

            return lstNetworks;
        }

        public Boolean ConnectToThermoStat(String ssid)
        {
            //Find current interface Guid
            WlanClient.WlanInterface intfc = GetCurrentInterface();
            //ssid = ssid.Replace("-", "");
            
            if (intfc != null)
            {
                //Profile Name - Create a new profile with profile name as Thermostats SSIS                                
                String profileXml = string.Format(@"<?xml version=""1.0"" encoding=""US-ASCII""?>
                    <WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
                        <name>{0}</name>
                        <SSIDConfig>
                            <SSID>                                
                                <name>{0}</name>                                
                            </SSID>
                        </SSIDConfig>
                        <connectionType>IBSS</connectionType>
                        <connectionMode>manual</connectionMode>
                        <autoSwitch>false</autoSwitch>
                        <MSM>
                            <security>
                                <authEncryption>
                                    <authentication>open</authentication>
                                    <encryption>none</encryption>
                                    <useOneX>false</useOneX>
                                </authEncryption>
                            </security>
                        </MSM>
                    </WLANProfile>", ssid);   

                intfc.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);

                intfc.WlanConnectionNotification += new WlanClient.WlanInterface.WlanConnectionNotificationEventHandler(test);


                //Connect to network
                Wlan.Dot11Ssid ID = new Wlan.Dot11Ssid();

                ID.SSIDLength = (uint)Encoding.ASCII.GetByteCount(ssid);
                ID.SSID  = Encoding.ASCII.GetBytes(ssid);

                //Assuming that Thermostat network is always adhoc
                //intfc.Connect(Wlan.WlanConnectionMode.TemporaryProfile, Wlan.Dot11BssType.Independent,ID , Wlan.WlanConnectionFlags.HiddenNetwork);
                return intfc.ConnectSynchronously(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, ssid, 15000);                
            }
            else 
            {
                throw new Exception("Guid not found");
            }
        }

        public static string encode(string ascii)
        {
            if (ascii == null)
            {
                return null;
            }
            else
            {
                char[] arrChars = ascii.ToCharArray();
                string binary = "";
                //string divider = ".";
                foreach (char ch in arrChars)
                {
                    binary += Convert.ToString(Convert.ToInt32(ch), 2);
                }
                return binary;
            }
        } 

        public void ConnectToHomeNetwork(JsonNetwork net, String pin, String password)
        {
            //Get UUID
            Thermostat t = getThermostatDetails();

            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pin, Encoding.UTF8.GetBytes(t.UUID), 1000);
            
            // generate an RC2 key
            byte[] key = pwdGen.GetBytes(16);
            byte[] iv = pwdGen.GetBytes(8);

            String pass = String.Empty;
            for (int i = 0; i < key.Length; i++)
            {
                pass += key[i].ToString();
            }

            pass = encode(pass);

            AesCryptoServiceProvider aes_cp = new AesCryptoServiceProvider();
            aes_cp.Mode = CipherMode.CBC;
            aes_cp.KeySize = 128;
            aes_cp.Key = key;
            aes_cp.IV = key;
            aes_cp.Padding = PaddingMode.PKCS7;

            // now encrypt with it
            byte[] plaintext = Encoding.UTF8.GetBytes(pass);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, aes_cp.CreateEncryptor(), CryptoStreamMode.Write);            
            

            cs.Write(plaintext, 0, plaintext.Length);
            cs.Close();
            byte[] encrypted = ms.ToArray();


            //Connect to network
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://192.168.10.1/sys/network");
            myReq.Method = @"PUT";
            myReq.ContentType = @"application/json; charset=utf-8";

            string Body = "{'ssid':" + net.Ssid +"','bssid':'" + net.Bssid + "','channel': '" + net.Channel +",'security': " + net.SecurityMode
                +",'ip':1,'rssi':'" + net.RSSI_in_dBm + "'}";

            byte[] data = Encoding.UTF8.GetBytes(Body);
            myReq.ContentLength = data.Length;

            //Request Stream
            Stream stream = myReq.GetRequestStream();
            stream.Write(data, 0, data.Length);

            Stream responseStream = myReq.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

            //Stream to String
            String result = reader.ReadToEnd();
            reader.Close();
        }

        private Thermostat getThermostatDetails()
        {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://192.168.10.1/sys");
            myReq.Method = @"GET";
            HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
            String result = reader.ReadToEnd();
            return new Thermostat(result,thermostatSSID) ;
        }

        public void ConnectToNetwork(JsonNetwork net)
        {
            WlanClient.WlanInterface intfc = GetCurrentInterface();
            String profileXML = GetProfileXML(net.Ssid);
            if (profileXML.CompareTo("not found") == 0)
            {

            }
            else {
                intfc.ConnectSynchronously(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, net.Ssid, 15000);
            }
        }

        void test(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
        {
            var = notifyData.NotificationCode;
            src = connNotifyData.wlanReasonCode; 
        }


        private Guid GetInterfaceGUID()
        {            
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                if(wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected)
                    return wlanIface.InterfaceGuid;
            }
            return Guid.Empty;
        }

        private WlanClient.WlanInterface GetCurrentInterface()
        {
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected)
                    return wlanIface;
            }
            return null;
        }

        public string getConnectedNetwork()
        {
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected)
                {
                    Guid interfaceGuid = wlanIface.InterfaceGuid;
                    return wlanIface.CurrentConnection.profileName;
                }
            }
            return string.Empty;
        }

        public object Var 
        {   get { return var;} 
            set {this.var = value;}
        }

        public List<JsonNetwork> fetchNetworks()
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
                return net;
                

            }
            catch (Exception)
            {
                throw new Exception("Could not fetch the network list from your thermostat.");
            }
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

        public string GetProfileXML(String profileName)
        {
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected)
                {
                    Wlan.WlanProfileInfo[] profs;
                    Guid interfaceGuid = wlanIface.InterfaceGuid;
                    profs = wlanIface.GetProfiles();

                    foreach (Wlan.WlanProfileInfo info in profs)
                    {
                        if (info.profileName.Equals(profileName))
                            return wlanIface.GetProfileXml(info.profileName);
                    }
                }
            }

            return "not found";
        }
    }
}
