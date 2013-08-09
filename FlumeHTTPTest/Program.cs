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
using FlumeHTTPTest;

namespace FlumeHTTPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int LinesToWrite = 5000;
            Console.WriteLine("Connecting to server...");
            FlumeHTTPSourceClient client = new FlumeHTTPSourceClient("http://172.16.195.159:1337");
            FlumeEvents events = new FlumeEvents();
            long timestamp = client.TimeStamp();
            for (int i = 1; i <= LinesToWrite; i++)
            {
                FlumeEvent toAdd = new FlumeEvent("my.windows.box", timestamp, String.Format("{0},this-is-a-test", i));
                events.Add(toAdd);
            }
            DateTime start = DateTime.Now;
            int code = client.Append(events);
            DateTime end = DateTime.Now;
            if (code == 0)
            {
                Console.WriteLine("Wrote {0} lines to Flume in {1} milliseconds", events.Count, (end - start).Milliseconds);
            }
            else
            {
                if (code == 200)
                {
                    Console.WriteLine("The request was not formatted properly");
                }
                else
                {
                    Console.WriteLine("There was an error.");
                }
            }
            Console.Write("Press return to exit");
            var name = Console.ReadLine();
        }
    }
}
