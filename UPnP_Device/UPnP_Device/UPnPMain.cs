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
        private const int Port = 52000;

        public string UUID { get; private set; }
        public string LocalIP { get; private set; }
        
        public UPnPMain()
        {
            UUID = IPHandler.GetGUID();
            LocalIP = IPHandler.GetOwnIp();

            udpServer = new UDPServer(UUID, cacheExpire, LocalIP, Port);
            SubscribeToSetupDoneEvent();
            udpServer.Start();         
        }

        private void SetupDoneEventHandler(object e, EventArgs args)
        {
            tcpServer = new TCPServer(/*port*/);
        }

        private void SubscribeToSetupDoneEvent()
        {
            //ToDo: Subscribe til event i udpServer
        }
    }


}
