using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UPnP_Device
{

    //Static Utility class for IP and ID purposes
    public class IPHandler
    {
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
            if(instance == null)
                instance = new IPHandler();
            return instance;
        }

        private static string GetGUID()
        {
            //var g = Guid.NewGuid();
            //return g.ToString();
            return "e22ca7c1-aadc-4d60-b334-2d905bef5be7";
        }

        private static string GetOwnIp()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                Console.WriteLine("Found ip: " + ip.ToString() + "family: " + ip.AddressFamily.ToString());
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine("ip: " + ip.ToString());
                    localIP = ip.ToString();
                }
            }
            return localIP;
            //return "127.0.0.1";
        }
    }
}
