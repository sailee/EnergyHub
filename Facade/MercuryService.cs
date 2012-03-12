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
                //String to Object
                JObject jsonObj = JObject.Parse(result);
                //Object to Token
                string mobileAuthToken = (string)jsonObj["value"];
          
                //Return Token
                return mobileAuthToken;
            }

            catch (WebException ex)
            {
                throw ex;
            }

            catch (Exception e)
            {
                throw e;
            }
        }       
    }
}