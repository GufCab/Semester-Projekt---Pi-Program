using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public string DeviceType { get; private set; }

        private IPHandler()
        {
            GUID = GetGUID();
            IP = GetOwnIp();

            PA = "\"HiPiSerial\"";
            MV = "\"1.00\"";
            MN = "\"HiPiSerial\"";
            CN = "\"Gruppe 8\"";
            AV = "\"5.0\"";

            DeviceType = "urn:schemas-upnp-org:device:MediaRenderer:1";
        }
        
        public static IPHandler GetInstance()
        {
            if(instance == null)
                return new IPHandler();
            return instance;
        }

        private static string GetGUID()
        {
            var g = Guid.NewGuid();
            return g.ToString();
        }

        private static string GetOwnIp()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
    }
}
