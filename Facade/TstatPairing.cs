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

        public static byte[] encode(string binary)
        {
            if (binary == null)
            {
                return null;
            }
            else
            {
                int numOfBytes = binary.Length / 8;
                byte[] bytes = new byte[numOfBytes];
                for (int i = 0; i < numOfBytes; ++i)
                {
                    bytes[i] = Convert.ToByte(binary.Substring(8 * i, 8), 2);
                }
                return bytes;
            }
        }
       
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        public Boolean ConfigureHomeNetwork(JsonNetwork net, String pin, String password)
        {
            try
            {
                //Get UUID
                Thermostat t = getThermostatDetails();

                // Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pin, Encoding.UTF8.GetBytes(uuid), 1000);
                Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pin, Encoding.UTF8.GetBytes(t.UUID), 1000);

                // generate an RC2 key
                byte[] key = pwdGen.GetBytes(16);
                byte[] iv = pwdGen.GetBytes(8);

                //Convert the network password from hex-ascii to binary
                //C17A0E3E5DD809B450FBF02B1E
                byte[] pass = Encoding.ASCII.GetBytes(password);

                String str = String.Empty;

                int missingDigits = 16 - pass.Length % 16;

                for (int i = 0; i < pass.Length + missingDigits; i++)
                {
                    if (i < pass.Length)
                        str += pass[i].ToString();
                    else
                        str += missingDigits.ToString();
                }

                byte[] cipher = EncryptStringToBytes_Aes(password, key, key);
                string hex = BitConverter.ToString(cipher);
                hex = hex.Replace("-", "");


                //Get Network configuration
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://192.168.10.1/sys/network");
                HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                //Stream to String
                String result = reader.ReadToEnd();
                reader.Close();

                JObject jsonObj = JObject.Parse(result);

                net.Ipaddr = (String)jsonObj["ipaddr"];
                net.Ipgw = (String)jsonObj["ipgw"];
                net.Ipmask =(String)jsonObj["ipmask"];

                result = pushNetworkConfiguration(net, hex);

                JObject ans = JObject.Parse(result);
                int res1 = (int)ans["success"];
                if (res1 == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }   
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private String pushNetworkConfiguration(JsonNetwork net, string key)
        {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://192.168.10.1/sys/network");
            
            myReq = (HttpWebRequest)WebRequest.Create("http://192.168.10.1/sys/network");
            myReq.Method = "POST";
            myReq.ContentType = @"application/json; charset=utf-8";

            String Body = getJson(net, key);

            byte[] data = Encoding.UTF8.GetBytes(Body);
            myReq.ContentLength = data.Length;

            //Request Stream
            Stream stream = myReq.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
            String status = response.StatusDescription;

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            //Stream to String
            String result= reader.ReadToEnd();

            reader.Close();
            responseStream.Close();
            response.Close();

            return result;
        }        

        private String getJson(JsonNetwork net, String key)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            //'{"version":"2.00","ssid":"Trantor","security":3,"key":"mysecret","ip":0,"ipaddr":"192.168.1.100","ipmask":"255.255.255.0","ipgw":"192.168.1.1"}'
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                jw.WriteStartObject();
                jw.WritePropertyName("version");
                jw.WriteValue("2"); //what is this???
                jw.WritePropertyName("ssid");
                jw.WriteValue(net.Ssid);
                jw.WritePropertyName("security");
                jw.WriteValue(net.SecurityMode);
                jw.WritePropertyName("key");
                jw.WriteValue(key);
                jw.WritePropertyName("ip");
                jw.WriteValue(1); //verify this, and about other missing fields
                jw.WritePropertyName("ipaddr");
                jw.WriteValue(net.Ipaddr);
                jw.WritePropertyName("ipmask");
                jw.WriteValue(net.Ipmask);
                jw.WritePropertyName("ipgw");
                jw.WriteValue(net.Ipgw);
                //jw.WriteEnd();
                jw.WriteEndObject();
            }
            return sb.ToString();
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
            //what if profileXML is not found?
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
