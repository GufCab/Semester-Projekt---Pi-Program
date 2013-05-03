using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device.UDP;
using UPnP_Device.UPnP;

namespace UPnP_Device
{
    public interface IUPnPMain
    {
        event UPnPEvents.PauseOrder PauseEvent;
    }
    
    public class UPnPMain : IUPnPMain
    {
        private TCPReceiver tcpReceiver;
        private UDPServer udpServer;

        private const int cacheExpire = 60; //Cache expire in seconds
        private const int port = 52000;
        
        public string UUID { get; private set; }
        public string localIP { get; private set; }

        //
        public event UPnPEvents.PauseOrder PauseEvent;

        public UPnPMain()
        {
            UUID = IPHandler.GetGUID();
            localIP = IPHandler.GetOwnIp();
            tcpReceiver = new TCPReceiver(localIP, port);

            udpServer = new UDPServer(UUID, cacheExpire, localIP, port);
            udpServer.Start();
        }
    }

    public class UPnPEventArgs : EventArgs
    {
        
    }


}
