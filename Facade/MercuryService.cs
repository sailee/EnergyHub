using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using InternalDataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Facade;
using System.Windows.Forms;


namespace Facade
{
    public class MercuryService
    {
        public String ObtainMobileAuthToken()
        {

            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://mercury-beta.energyhub.net/filtrete/rest/public/mobileAuth");
                myReq.Credentials = new NetworkCredential("sean.helvey@gmail.com", "Br@infood11");
                myReq.Method = @"POST";
                myReq.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                string Body = "j_username=sean.helvey@gmail.com&j_password=Br@infood11";
                byte[] data = Encoding.UTF8.GetBytes(Body);
                myReq.ContentLength = data.Length;
                using(Stream stream = myReq.GetRequestStream())
                    stream.Write(data, 0, data.Length);
                using(Stream responseStream = myReq.GetResponse().GetResponseStream())
                using(StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string result = reader.ReadToEnd();
                }

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
                string authToken = JsonMercuryDeserialize(sb.ToString());
                return authToken;
            }

            catch (WebException Exception1)
            {
                throw new WebException("Could not obtain auth token from server.", Exception1);
            }

            catch (Exception Exception2)
            {
                throw new Exception("Could not obtain auth token from server.", Exception2);
            }
        }

        private string JsonMercuryDeserialize(string json)
        {
            //JObject jsonObj = JObject.Parse(json);

            //JArray networks = (JArray)jsonObj["networks"];

            //List<JsonNetwork> nets = new List<JsonNetwork>();

            //foreach (JToken token in networks)
            //{
            //    String ssid = (String)token[0];
            //    String bssid = (String)token[1];
            //    int secMode = (int)token[2];
            //    int Channel = (int)token[3];
            //    int rssi = (int)token[4];

            //    JsonNetwork net = new JsonNetwork(ssid, bssid, secMode, Channel, rssi);
            //    nets.Add(net);
            //}
            return json;
        }
    }
}