using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NativeWifi;
using InternalDataTypes;
using System.Collections;

namespace Facade
{
    public class TstatPairing
    {
        private WlanClient client;
        private string thermostatSSID;

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

        public void ConnectToThermoStat(String ssid)
        {
            //Find current interface Guid
            WlanClient.WlanInterface intfc = GetCurrentInterface();
            //ssid = ssid.Replace("-", "");
            
            if (intfc != null)
            {
                //Profile Name - Create a new profile with profile name as Thermostats SSIS                                
//                String profileXml = string.Format(@"<?xml version=""1.0"" encoding=""US-ASCII""?>
//                    <WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
//                        <name>{0}</name>
//                        <SSIDConfig>
//                            <SSID>                                
//                                <name>{0}</name>
//                                <nonBroadcast>false</nonBroadcast>
//                            </SSID>
//                        </SSIDConfig>
//                        <connectionType>IBSS</connectionType>
//                        <connectionMode>manual</connectionMode>
//                        <autoSwitch>false</autoSwitch>
//                        <MSM>
//                            <security>
//                                <authEncryption>
//                                    <authentication>open</authentication>
//                                    <encryption>none</encryption>
//                                    <useOneX>false</useOneX>
//                                </authEncryption>
//                            </security>
//                        </MSM>
//                    </WLANProfile>", ssid);

                String profileXml = string.Format(@"<?xml version=""1.0"" encoding=""US-ASCII""?>
                    <WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
                        <name>{0}</name>
                        <SSIDConfig>
                            <SSID>                                
                                <name>{0}</name>                                
                            </SSID>
                        </SSIDConfig>
                        <connectionType>ESS</connectionType>
                        <connectionMode>auto</connectionMode>
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

                //Connect to network
                Wlan.Dot11Ssid ID = new Wlan.Dot11Ssid();

                ID.SSIDLength = (uint)Encoding.ASCII.GetByteCount(ssid);
                ID.SSID  = Encoding.ASCII.GetBytes(ssid);

                //Assuming that Thermostat network is always adhoc
                //intfc.Connect(Wlan.WlanConnectionMode.TemporaryProfile, Wlan.Dot11BssType.Independent,ID , Wlan.WlanConnectionFlags.HiddenNetwork);
                intfc.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, ssid);

                //intfc.Connect(
                
                    //(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Independent, ssid);
            }
            else 
            {
                throw new Exception("Guid not found");
            }
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
    }
}
