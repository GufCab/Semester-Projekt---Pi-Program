using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPnP_Device.UDP
{
    public class UDPServer
    {
        private string _UUID;
        private int _cacheexpire;
        private string _localip;
        private int _tcpport;
        
        //private static readonly IPAddress multicastIp = IPAddress.Parse("239.255.255.250");
        //private static readonly int multicastPort = 1900;

        public MulticastSender sender;
        public MulticastReceiver receiver;

        private Thread NotifyThread;
        private Thread ReceiveThread;

        
        public UDPServer(string uuid, int cacheexpire, string localip, int tcpport)
        {
            _UUID = uuid;
            _cacheexpire = cacheexpire;
            _localip = localip;
            _tcpport = tcpport;

            sender = new MulticastSender(_UUID, _cacheexpire, _localip, _tcpport);

            NotifyThread = new Thread(sender.NotifySender);
        }

        public void Start()
        {
            NotifyThread.Start();
        }

    }
}
