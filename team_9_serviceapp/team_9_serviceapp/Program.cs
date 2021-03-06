﻿using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SimpleWebServer;
using System.Diagnostics;

public class GetSocket
{
    private static Socket ConnectSocket(string server, int port)
    {
        Socket s = null;
        IPHostEntry hostEntry = null;

        // Get host related information.
        hostEntry = Dns.GetHostEntry(server);

        

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
        foreach (IPAddress address in hostEntry.AddressList)
        {
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket =
                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
            {
                s = tempSocket;
                break;
            }
            else
            {
                continue;
            }
        }
        return s;
    }

    // This method requests the home page content for the specified server.
    private static string SocketSendReceive(string server, int port)
    {
        string request = "GET / HTTP/1.1\r\nHost: " + server +
            "\r\nConnection: Close\r\n\r\n";
        Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
        Byte[] bytesReceived = new Byte[1024];
        string page = "";

        // Create a socket connection with the specified server and port.
        using (Socket s = ConnectSocket(server, port))
        {
            if (s == null)
                return ("Connection failed");

            // Send request to the server.
            s.Send(bytesSent, bytesSent.Length, 0);

            // Receive the server home page content.
            int bytes = 0;
            
            // The following will block until the page is transmitted.
            do
            {
                bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
            }
            while (bytes > 0);
        }
        return page;
    }

    public static void Main(string[] args)
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
        string host = "www.google.com";
        int port = 80;
        // return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);

        string output = SocketSendReceive(host, port);
        Console.WriteLine(output);
        string result = output.Substring(output.IndexOf("<!doctype"));

        return result;
    }
}