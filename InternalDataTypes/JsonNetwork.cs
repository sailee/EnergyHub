using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace InternalDataTypes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonNetwork
    {

        private string ssid, ipaddr, ipgw, ipmask;  
       
        public string Ssid
        {
            get { return ssid; }
            set { ssid = value; }
        }

        //[DataMember(Order = 1)]
        //[JsonProperty()]
        public string Bssid;
         //[DataMember(Order = 2)]
        //[JsonProperty()]
        public int SecurityMode;
         //[DataMember(Order = 3)]
        //[JsonProperty()]
        public int Channel;
        //[DataMember(Order = 4)]
        //[JsonProperty()]
        public int RSSI_in_dBm;       
        
        
        public JsonNetwork()
        { }

        public JsonNetwork(string ssid, string bssid, int securityMode, int channel, int rSSI)
        {
            this.ssid = ssid;
            this.Bssid = bssid;
            this.SecurityMode = securityMode;
            this.Channel = channel;
            this.RSSI_in_dBm = rSSI;
        }

        public String ToString()
        {
            String str = "SSID: " + ssid + "\tBSSID: " + Bssid;
                return str;
        }

        public String Ipmask
        {
            get { return ipmask; }
            set { ipmask = value; }
        }

        public String Ipgw
        {
            get { return ipgw; }
            set { ipgw = value; }
        }

        public String Ipaddr
        {
            get { return ipaddr; }
            set { ipaddr = value; }
        }
    }
}
