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
        event UPnPEvents.PlayOrder PlayEvent;
        event UPnPEvents.SetTransportURIOrder SetTransportURIEvent;
    }

    public class UPnPMain : IUPnPMain
    {
        private TCPReceiver tcpReceiver;
        private UDPServer udpServer;

        private const int cacheExpire = 60; //Cache expire in seconds
        private const int port = 52000;

        public string UUID { get; private set; }
        public string localIP { get; private set; }

        //Events defined in UPnPEvents class
        public event UPnPEvents.PauseOrder PauseEvent;
        public event UPnPEvents.PlayOrder PlayEvent;
        public event UPnPEvents.SetTransportURIOrder SetTransportURIEvent;


        public UPnPMain()
        {
            IPHandler ipHandler = IPHandler.GetInstance();

            UUID = IPHandler.GetInstance().GUID;
            localIP = IPHandler.GetInstance().IP;

            tcpReceiver = new TCPReceiver(localIP, port);

            udpServer = new UDPServer(UUID, cacheExpire, localIP, port);
            udpServer.Start();

        }
    }

    public class UPnPEventArgs : EventArgs
    {

    }
}
