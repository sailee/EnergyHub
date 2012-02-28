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
        private object var,src;


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
                intfc.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, ssid);
                String str1,str2;

                //while (object.Equals(var, null) || object.Equals(src,null))
                //{
                    
                //}                
                //while (String.Compare("Connected", var.ToString()) !=0)
                //{
                //    str1 = var.ToString();
                //    str2 = src.ToString();
                //}

                while (intfc.InterfaceState != Wlan.WlanInterfaceState.Connected)
                {
                    str1 = intfc.InterfaceState.ToString();
                }
                str2 = var.ToString();
            }
            else 
            {
                throw new Exception("Guid not found");
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

        //public string GetProfileXML()
        //{
        //    foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
        //    {
        //        if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected)
        //        {
        //            Wlan.WlanProfileInfo[] profs;        
        //            Guid interfaceGuid = wlanIface.InterfaceGuid;
        //            profs = wlanIface.GetProfiles();

        //            foreach (Wlan.WlanProfileInfo info in profs)
        //            {
        //                if(info.profileName.StartsWith("thermostat"))
        //                    return info.profileName + "\n" + wlanIface.GetProfileXml(info.profileName);
        //            }
        //        }
        //    }

        //    return "not found";
        //}
    }
}
