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

        protected override void OnStart(string[] args)
        {
            WebSer ws = new WebSer(SendResponse, "http://localhost:8080/test/");
            ws.Run();
        }

        protected override void OnStop()
        {
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            string url = "https://github.com/buryo";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());

            string result = stream.ReadToEnd();

            return result;
        }
    }
}
