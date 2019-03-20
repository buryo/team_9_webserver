using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;

namespace Webserver
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        // The start point of the application
        protected override void OnStart(string[] args)
        {
            WebSer ws = new WebSer(SendRequest, "http://localhost:8080/test/");
            ws.Run();
        }

        // Method to send request
        public static string SendRequest(HttpListenerRequest request)
        {
            // Change the URL to visit other websites
            string url = "https://github.com/buryo";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());

            string result = stream.ReadToEnd();

            return result;
        }
    }
}
