using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UPnP_Device.UPnPConfig
{
    public interface IIpConfig
    {
        string GUID { get; }
        string IP { get; }

        int TCPPort { get; }
    }

    public class SinkIPConfig : IIpConfig
    {
        public string GUID { get; private set; }
        public string IP { get; private set; }
        public int TCPPort { get; private set; }

        public SinkIPConfig(int tcpportnr)
        {
            GUID = GetGUID();
            IP = GetOwnIp();
            TCPPort = tcpportnr;
        }

        private string GetGUID()
        {
			Console.WriteLine ("Getting GUID");

            string g;
            const string folderpath = @"../../config/";
            const string filename = "SinkGUID.key";
            const string fullpath = folderpath + filename;

            if (File.Exists(fullpath))
            {
                using (var sr = new StreamReader(fullpath, Encoding.UTF8))
                {
                    g = sr.ReadLine();
                    sr.Close();
                }
            }
            else
            {
                g = Guid.NewGuid().ToString();

                if (Directory.Exists(folderpath) == false)
                    Directory.CreateDirectory(folderpath);
                using (var fs = new FileStream(fullpath, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(g);
                    sw.Close();
                }
            }
            return g; //"e22ca7c1-aadc-4d60-b334-2d905bef5be7";
        }

        private string GetOwnIp()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

    }

    public class SourceIPConfig : IIpConfig
    {
        public string GUID { get; private set; }
        public string IP { get; private set; }
        public int TCPPort { get; private set; }

        public SourceIPConfig(int tcpportnr)
        {
            GUID = GetGUID();
            IP = GetOwnIp();
            TCPPort = tcpportnr;
        }

        private string GetGUID()
        {
            string g;
            const string folderpath = @"../../config/";
            const string filename = "SourceGUID.key";
            const string fullpath = folderpath + filename;

            if (File.Exists(fullpath))
            {
                using (var sr = new StreamReader(fullpath, Encoding.UTF8))
                {
                    g = sr.ReadLine();
                    sr.Close();
                }
            }
            else
            {
                g = Guid.NewGuid().ToString();

                if (Directory.Exists(folderpath) == false)
                    Directory.CreateDirectory(folderpath);
                using (var fs = new FileStream(fullpath, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(g);
                    sw.Close();
                }
            }
            return g;
        }

        private string GetOwnIp()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
    }
}
