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
    //Service for interaction with the Mercury server
    public class MercuryService
    {
        public static string Token = "";

        //HttpWebRequest to Mercury posting credentials to retrieve token
        public String ObtainMobileAuthToken()
        {
            try
            {
                //Web Request
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://mercury-beta.energyhub.net/filtrete/rest/public/mobileAuth");

                //Credentials, Method, ContentType
                myReq.Credentials = new NetworkCredential("sean.helvey@gmail.com", "Br@infood11");
                myReq.Method = @"POST";
                myReq.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";

                //Data, Encoding, Size
                string Body = "j_username=sean.helvey@gmail.com&j_password=Br@infood11";
                byte[] data = Encoding.UTF8.GetBytes(Body);
                myReq.ContentLength = data.Length;

                //Request Stream
                Stream stream = myReq.GetRequestStream();
                stream.Write(data, 0, data.Length);

                //Response Stream
                Stream responseStream = myReq.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                //Stream to String
                string result = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                //String to Object
                JObject jsonObj = JObject.Parse(result);
                //Object to Token
                string mobileAuthToken = (string)jsonObj["value"];
                Token = mobileAuthToken;

                //Return Token
                return mobileAuthToken;
            }

            catch (WebException)
            {
                throw new WebException("WebException");
            }

            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }
        //HttpWebRequest to fetch locations from Mercury
        public string[] FetchLocations()
        {
            try
            {
                //Web Request
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://mercury-beta.energyhub.net/filtrete/rest/locations");

                myReq.Headers.Add("X-Mobile-Auth", Token);

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

                string[] returnEm = jsonParse(sb);

                //return locations
                return returnEm;
            }

            catch (WebException)
            {
                throw new WebException("WebException");
            }

            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        public string[] jsonParse(StringBuilder mySB)
        {

            string result = mySB.ToString();
            string[] splitter = result.Split(',');
            string[] subSplit1;
            string[] subSplit2;
            List<string> holder = new List<string>();
            List<string> keepers = new List<string>();

            foreach (string stringy in splitter)
            {
                if (stringy.Contains("name"))
                {
                    subSplit1 = stringy.Split(':');
                    foreach (string stringle in subSplit1)
                    {
                        subSplit2 = stringle.Split('"');
                        foreach (string fo in subSplit2)
                        {
                            holder.Add(fo);
                        }
                    }
                    keepers.Add(holder[4]);
                }
            }

            string[] returnVals = keepers.ToArray();
            return returnVals;
        }
    }
}