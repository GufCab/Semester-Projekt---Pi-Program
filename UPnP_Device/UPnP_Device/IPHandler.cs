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

        private IPHandler()
        {
            GUID = GetGUID();
            IP = GetOwnIp();
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
