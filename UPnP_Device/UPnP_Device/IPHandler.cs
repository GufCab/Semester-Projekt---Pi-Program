using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace UPnP_Device
{

    //Static Utility class for IP and ID purposes
    public static class IPHandler
    {
        public static string GetGUID()
        {
            var g = Guid.NewGuid();
            return g.ToString();
        }

        public static string GetOwnIp()
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
