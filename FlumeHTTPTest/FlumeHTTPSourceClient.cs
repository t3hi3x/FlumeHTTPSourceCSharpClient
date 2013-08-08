/*
 * Copyright 2013 Cloudera, Inc.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;


namespace FlumeHTTPTest
{
    // Class for defifing a flume event. It must formatted properly in JSON for the JSON Handler. See http://flume.apache.org/FlumeUserGuide.html#jsonhandler
    public class FlumeEvent
    {
        private string _host;
        private double _timestamp;
        private string _body;
        public FlumeEvent(string host, double timestamp, string body)
        {
            this._host = host;
            this._timestamp = timestamp;
            this._body = body;
        }

        public override string ToString()
        {
            string toReturn = String.Format(@"{{""headers"":{{""timestamp"":""{0}"",""host"":""{1}""}},""body"":""{2}""}}", Convert.ToString(this._timestamp), this._host, this._body);
            return toReturn;
        }
    }

    // Inherited class for overriding the toString() method allowing a JSON array of FlumeEvents to be returned.
    public class FlumeEvents : List<FlumeEvent>
    {
        public override string ToString()
        {
            string toReturn = "[";
            FlumeEvent first = this.First();
            foreach (FlumeEvent i in this)
            {
                if (i == first)
                {
                    toReturn += i.ToString();
                }
                else
                {
                    toReturn += ",";
                    toReturn += i.ToString();
                }
            }
            toReturn += "]";
            return toReturn;
        }
    }

    class FlumeHTTPSourceClient
    {
        // URI for submitting the request to. Example: "http://example.com:1337"
        private string _uri;

        public string URI
        {
            get {return _uri;}
        }

        public FlumeHTTPSourceClient(string URI)
        {
            this._uri = URI;
        }

        public long TimeStamp()
        {
            return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }

        // Generic HTTP POST
        private HttpWebResponse _post(string data)
        {
            //TODO: Implimenet Security?
            WebRequest wb = WebRequest.Create(this._uri);
            wb.Method = "POST";
            wb.ContentType = "application/json";
            byte[] toPost = Encoding.UTF8.GetBytes(data);
            wb.ContentLength = toPost.Length;
            Stream dataStream = wb.GetRequestStream();
            dataStream.Write(toPost, 0, toPost.Length);
            dataStream.Close();
            return (HttpWebResponse)wb.GetResponse();
        }

        // Sends the Flume Events to the server
        public int Append(FlumeEvents FlumeEventsToSend)
        {
            try
            {
                var attempt = _post(FlumeEventsToSend.ToString());
                if (attempt.StatusCode == HttpStatusCode.OK)
                {
                    return 0;
                }
                else if (attempt.StatusCode == HttpStatusCode.BadRequest)
                {
                    return 400;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return 2;
            }
            
        }
    }
}
