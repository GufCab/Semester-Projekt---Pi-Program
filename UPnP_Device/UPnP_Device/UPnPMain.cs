using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device
{
    interface IUPnPMain
    {
        
    }

    class UPnPMain : IUPnPMain
    {
        private TCPServer tcpServer;
        private UDPServer udpServer;

        private const int cacheExpire = 1800;

        public string UUID { get; private set; }
        public string localIP { get; private set; }

        public UPnPMain()
        {
            UUID = IPHandler.GetGUID();
            localIP = IPHandler.GetOwnIp();

            udpServer = new UDPServer(UUID, cacheExpire, localIP);
            SubscribeToSetupDoneEvent();
            udpServer.Start();



        }

        private void SubscribeToSetupDoneEvent()
        {
            //ToDo: Subscribe til event i udpServer
        }
    }


}
