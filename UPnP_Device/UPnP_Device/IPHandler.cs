using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPnP_Device
{

    //Static Utility class for IP and ID purposes
    public class IPHandler
    {
        private static Mutex mu = new Mutex();

        private static IPHandler instance = null;

        public string GUID { get; private set; }
        public string IP { get; private set; }

        public string AV { get; private set; }
        public string CN { get; private set; }
        public string MN { get; private set; }
        public string MV { get; private set; }
        public string PA { get; private set; }

        public string DeviceSchema { get; private set; }
        public string DeviceType { get; private set; }

        private IPHandler()
        {

            GUID = GetGUID();
            IP = GetOwnIp();
            //IP = "127.0.0.1";

            PA = "\"HiPiSerial\"";
            MV = "\"1.00\"";
            MN = "\"HiPiSerial\"";
            CN = "\"Gruppe 8\"";
            AV = "\"5.0\"";

            DeviceSchema = "urn:schemas-upnp-org:device::";
            DeviceType = "upnp:rootdevice";
        }
        
        public static IPHandler GetInstance()
        {

            if (instance == null)
            {
                mu.WaitOne();
                if (instance == null)
                    instance = new IPHandler();
                mu.ReleaseMutex();
            }
            return instance;
        }

        private static string GetGUID()
        {
            string g;
            string path = @"config/guid.key";
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    g = sr.ReadLine();
                    sr.Close();
                }
            }
            else
            {
                g = Guid.NewGuid().ToString();

                if (Directory.Exists("config") == false)
                    Directory.CreateDirectory("config");
                using (var fs = new FileStream(@"config/guid.key", FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(g);
                    sw.Close();
                }
            }
             

            return g;//"e22ca7c1-aadc-4d60-b334-2d905bef5be7";
        }

        private static string GetOwnIp()
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
