using NativeWifi;
using System.Text;

namespace InternalDataTypes
{
    /// <summary>
    /// Contains the profile name associated with the network.
    /// If the network doesn't have a profile, this member will be empty.
    /// If multiple profiles are associated with the network, there will be multiple entries with the same SSID in the visible network list. Profile names are case-sensitive.
    /// </summary>
    public class Network
    {
        private string profileName, dot11Ssid, dot11BssType, wlanNotConnectableReason, defaultAuthAlgorithm, defaultCipherAlgo;
        private uint numberOfBssids, numberOfPhyTypes, wlanSignalQuality;
        private bool securityEnabled, isConnectable;

        public Network(Wlan.WlanAvailableNetwork network)
        { 
            profileName = network.profileName;
            dot11Ssid = Encoding.ASCII.GetString(network.dot11Ssid.SSID, 0, (int)network.dot11Ssid.SSIDLength); 
            dot11BssType = network.dot11BssType.ToString(); 
            wlanNotConnectableReason = network.wlanNotConnectableReason.ToString(); 
            defaultAuthAlgorithm= network.dot11DefaultAuthAlgorithm.ToString(); 
            defaultCipherAlgo= network.dot11DefaultCipherAlgorithm.ToString();
            numberOfBssids = network.numberOfBssids; 
            //numberOfPhyTypes = network.Dot11PhyTypes.GetLength(network); 
            wlanSignalQuality = network.wlanSignalQuality;
            securityEnabled = network.securityEnabled;
            isConnectable = network.networkConnectable;
        }

        public string ProfileName
        {
            get { return profileName; }
        }

        /// <summary>
        /// Contains the SSID of the visible wireless network.
        /// </summary>
        public string NetworkSSID
        {
            get {return dot11Ssid;}
        }
        
        /// <summary>
        /// Specifies whether the network is infrastructure or ad hoc.
        /// </summary>
        public string NetworkType
        {
            get { return dot11BssType; }
        }
        
        /// <summary>
        /// Indicates the number of BSSIDs in the network.
        /// </summary>
        public uint BSSIDCount
        {
            get { return numberOfBssids; }
        }
        
        /// <summary>
        /// Indicates whether the network is connectable or not.
        /// </summary>
        public bool isNetworkConnectable
        {
            get { return isConnectable; }
        }
        
        /// <summary>
        /// Indicates why a network cannot be connected to. This member is only valid when <see cref="networkConnectable"/> is <c>false</c>.
        /// </summary>
        public string NoConnectionReason
        {
            get { return wlanNotConnectableReason; }
        }
        
        /// <summary>
        /// The number of PHY types supported on available networks.
        /// The maximum value of this field is 8. If more than 8 PHY types are supported, <see cref="morePhyTypes"/> must be set to <c>true</c>.
        /// </summary>
        public uint CountOfPhysicalTypes
        {
            get { return numberOfBssids; }
        }

        /// <summary>
        /// A percentage value that represents the signal quality of the network.
        /// This field contains a value between 0 and 100.
        /// A value of 0 implies an actual RSSI signal strength of -100 dbm.
        /// A value of 100 implies an actual RSSI signal strength of -50 dbm.
        /// You can calculate the RSSI signal strength value for values between 1 and 99 using linear interpolation.
        /// </summary>
        public uint SignalStrength
        {
            get { return wlanSignalQuality; }
        }

        /// <summary>
        /// Indicates whether security is enabled on the network.
        /// </summary>
        public bool isSecurityEnabled
        {
            get { return securityEnabled; }
        }
        
        /// <summary>
        /// Indicates the default authentication algorithm used to join this network for the first time.
        /// </summary>
        public string DefaultAuthenticationAlgo
        {
            get { return defaultAuthAlgorithm; }
        }
        
        /// <summary>
        /// Indicates the default cipher algorithm to be used when joining this network.
        /// </summary>
        public string DefaultCipherAlgo
        {
            get { return defaultCipherAlgo; }
        }

        /*
        /// <summary>
        /// Contains various flags for the network.
        /// </summary>
        public WlanAvailableNetworkFlags flags;
        /// <summary>
        /// Reserved for future use. Must be set to NULL.
        /// </summary>
        uint reserved;

        
        /// <summary>
        /// Contains an array of <see cref="Dot11PhyType"/> values that represent the PHY types supported by the available networks.
        /// When <see cref="numberOfPhyTypes"/> is greater than 8, this array contains only the first 8 PHY types.
        /// </summary>
        
        private Dot11PhyType[] dot11PhyTypes;
        /// <summary>
        /// Gets the <see cref="Dot11PhyType"/> values that represent the PHY types supported by the available networks.
        /// </summary>
        public Dot11PhyType[] Dot11PhyTypes
        {
            get
            {
                Dot11PhyType[] ret = new Dot11PhyType[numberOfPhyTypes];
                Array.Copy(dot11PhyTypes, ret, numberOfPhyTypes);
                return ret;
            }
        }      
         
         
      
        /// <summary>
        /// Specifies if there are more than 8 PHY types supported.
        /// When this member is set to <c>true</c>, an application must call <see cref="WlanClient.WlanInterface.GetNetworkBssList"/> to get the complete list of PHY types.
        /// <see cref="WlanBssEntry.phyId"/> contains the PHY type for an entry.
        /// </summary>
        public bool morePhyTypes;
        
 */

    }
}