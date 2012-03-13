using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace InternalDataTypes
{   
    public class Thermostat
    {
        String ssid, uuid, fw_version, wlan_fw_version;    
        int api_version ;

        public Thermostat(String ssid, String uuid, int api_version, String fw_version, String wlan_fw_version)
        {
            this.ssid = ssid;
            this.uuid = uuid;
            this.api_version = api_version;
            this.fw_version = fw_version;
            this.wlan_fw_version = wlan_fw_version;
        }

        public Thermostat(String json, String ssid)
        {
            JObject jsonObj = JObject.Parse(json);

            this.ssid = ssid;
            this.uuid = (String)jsonObj["uuid"];
            this.api_version = (int)jsonObj["api_version"];
            this.fw_version = (String)jsonObj["fw_version"];
            this.wlan_fw_version = (String)jsonObj["wlan_fw_version"];
        }

        

        public String WlanFirmwareVersion
        {
            get { return wlan_fw_version; }
            set { wlan_fw_version = value; }
        }

        public String Firmware_version
        {
            get { return fw_version; }
            set { fw_version = value; }
        }

        public int Api_version
        {
            get { return api_version; }
            set { api_version = value; }
        }

        public String UUID
        {
            get { return uuid; }
            set { uuid = value; }
        }

        public String SSID
        {
            get { return ssid; }
            set { ssid = value; }
        }       
    }
}
