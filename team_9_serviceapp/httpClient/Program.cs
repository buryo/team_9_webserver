using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace httpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/test/");
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Process.Start("http://localhost:8080/test/");
            Console.ReadKey();
            ws.Stop();
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            string url = "https://github.com/buryo";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());
            Console.WriteLine("\nThe 'ProtocolVersion' of the protocol used is {0}", req.ProtocolVersion);

            string result = stream.ReadToEnd();
            return result;
        }
    }
}
