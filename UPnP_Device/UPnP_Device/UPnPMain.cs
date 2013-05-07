using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UPnP_Device.TCP;
using UPnP_Device.UDP;
using UPnP_Device.UPnP;


namespace UPnP_Device
{


    public class UPnPMain
    {
        private TCPReceiver tcpReceiver;
        private UDPServer udpServer;

        private TCPServer tcpServer;

        private const int cacheExpire = 1800; //Cache expire in seconds
        private const int port = 52000;

        public string UUID { get; private set; }
        public string localIP { get; private set; }

        public UPnPMain()
        {
            UUID = IPHandler.GetInstance().GUID;
            localIP = IPHandler.GetInstance().IP;

            tcpServer = new TCPServer(localIP, port);

            udpServer = new UDPServer(UUID, cacheExpire, localIP, port);
            udpServer.Start();

        }
    }
}
