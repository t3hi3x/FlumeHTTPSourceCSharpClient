FlumeHTTPSourceCSharpClient
===========================
Simple HTTPSource Client for Flume.

Currently, it only supports the JSON handler.

Sample Usage:


FlumeHTTPSourceClient client = new FlumeHTTPSourceClient("http://<server>:<port>");
FlumeEvents events = new FlumeEvents();
long timestamp = client.TimeStamp();
FlumeEvent toAdd = new FlumeEvent("my.windows.box", timestamp, String.Format("{0},this-is-a-test", i));
events.Add(toAdd);

client.Append(events);
