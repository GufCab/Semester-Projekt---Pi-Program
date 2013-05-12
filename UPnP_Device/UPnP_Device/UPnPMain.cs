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

        private const int cacheExpire = 1800; //Cache expire in seconds
        private const int port = 52000;

        public string UUID { get; private set; }
        public string localIP { get; private set; }

        //Events defined in UPnPEvents class
        public event UPnPEvents.PauseOrder PauseEvent;
        public event UPnPEvents.PlayOrder PlayEvent;
        public event UPnPEvents.SetTransportURIOrder SetTransportURIEvent;
        
        private TcpServer server;

        public UPnPMain()
        {
            UUID = IPHandler.GetInstance().GUID;
            localIP = IPHandler.GetInstance().IP;
            Console.WriteLine("main ip: " + localIP);

            server = new TcpServer(localIP, port);

            udpServer = new UDPServer(UUID, cacheExpire, localIP, port);
            udpServer.Start();

            TCP.EventContainer.PlayEvent += new TCP.EventContainer.PlayOrderHandler(ListenToPlay);
            TCP.EventContainer.NextEvent += new TCP.EventContainer.NextOrderHandler(ListenToNext);
            TCP.EventContainer.StopEvent += new TCP.EventContainer.StopOrderHandler(ListenToStop);

        }

        private void ListenToPlay(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Play was called from main class!");
            PlayEvent(this, args);

        }

        private void ListenToNext(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Next was called from main class!");
        }

        private void ListenToStop(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Stop was called from main class!");
            PlayEvent(this, args);
        }
    }

    public class UPnPEventArgs : EventArgs
    {

    }
}
