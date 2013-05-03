using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPnP_Device.UDP
{
    public class MulticastReceiver
    {
        //Local variables
        private string _UUID;
        private int _cacheexpire;
        private string _localip;
        private int _tcpport;

        //multicast variables:
        private static readonly IPAddress multicastIp = IPAddress.Parse("239.255.255.250");
        private static readonly int multicastPort = 1900;

        private static UdpClient recClient;
        private static IPEndPoint recIPep;
        private static Byte[] recBuffer;

         

        //Contructor:
        public MulticastReceiver(string uuid, int cacheexpire, string localip, int tcpport)
        {
            //Local variables
            _UUID = uuid;
            _cacheexpire = cacheexpire;
            _localip = localip;
            _tcpport = tcpport;   
        }

        private static void SetupMulticastReceiver()
        {
            recClient = new UdpClient();

            recIPep = new IPEndPoint(IPAddress.Any, multicastPort);
            recClient.Client.Bind(recIPep);

            recClient.JoinMulticastGroup(multicastIp);
        }

        //Blocks until reception
        public string ReceiveMulticast()
        {
            Byte[] data = recClient.Receive(ref recIPep);
            string strData = Encoding.UTF8.GetString(data);

            return strData;
        }


    }
}
