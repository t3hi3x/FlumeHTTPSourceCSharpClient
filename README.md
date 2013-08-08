FlumeHTTPSourceCSharpClient
===========================
Simple HTTPSource Client for Flume.

Program.cs               An exmaple client that adds 5000 lines in a single request.
FlumeHTTPSourceClient.cs  A series of classes for working with Flume via an HTTPSource.
ExampleFlume.conf        A sample Flume Configuration that works with this sample.

Currently, it only supports the JSON handler.

Sample Usage:
```
  FlumeHTTPSourceClient client = new FlumeHTTPSourceClient("http://<server>:<port>"); 
  FlumeEvents events = new FlumeEvents(); 
  long timestamp = client.TimeStamp(); 
  FlumeEvent toAdd = new FlumeEvent("my.windows.box", timestamp, String.Format("{0},this-is-a-test", i)); 
  events.Add(toAdd); 

  client.Append(events);
```
