using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace InternalDataTypes
{
   // [DataContract]
    public class JsonNets
    {
     //   [DataMember]
        [JsonProperty(PropertyName="networks")]
        public IList<JsonNetwork> networks;       

        
        public JsonNets()
        {
            networks = new List<JsonNetwork>();
        }
    }
}
