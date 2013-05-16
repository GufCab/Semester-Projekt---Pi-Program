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
        
    }

    public class TransportEventContainer
    {
        
    }

    public class UPnPMain : IUPnPMain
    {
        //private TCPReceiver tcpReceiver;
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

            XMLWriter1 wr = new XMLWriter1();

            wr.genDeviceDescription();
            wr.genServiceXmlAVTransport();
            //wr.genServiceXmlConnectionManager();

            server = new TcpServer(localIP, port);

            udpServer = new UDPServer(UUID, cacheExpire, localIP, port);
            udpServer.Start();

            TCP.EventContainer.PlayEvent += new TCP.EventContainer.PlayOrderHandler(ListenToPlay);
            TCP.EventContainer.NextEvent += new TCP.EventContainer.NextOrderHandler(ListenToNext);
            TCP.EventContainer.StopEvent += new TCP.EventContainer.StopOrderHandler(ListenToStop);
            TCP.EventContainer.PauseEvent += new TCP.EventContainer.PauseOrderHandler(ListenToPause);
            TCP.EventContainer.PreviousEvent += new TCP.EventContainer.PreviousOrderHandler(ListenToPrevious);
            TCP.EventContainer.SetAVTransportURIEvent += new TCP.EventContainer.SetAVTransportURIOrderHandler(ListenToSetAVTransport);

        }

        private void ListenToPlay(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Play was called from main class!");

            Console.WriteLine("The action was: " + args.Action);
            Console.WriteLine("With parameters: ");
            foreach (var tup in args.ArgList)
            {
                Console.WriteLine("Parameter: " + tup.Item1 + " Has value: " + tup.Item2);
            }

            //Raise interface event..
        }

        private void ListenToNext(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Next was called from main class!");
            //Raise interface event..
        }

        private void ListenToStop(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Stop was called from main class!");
            //Raise interface event..
        }

        private void ListenToPause(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Pause was called from main class!");
            //Raise interface event..
        }

        private void ListenToPrevious(object e, UPnPEventArgs args)
        {
            Console.WriteLine("Previous was called from main class!");
        }

        private void ListenToSetAVTransport(object e, UPnPEventArgs args)
        {
            Console.WriteLine("SetAVTransport was called from main class!");
        }
    }

    public class UPnPEventArgs : EventArgs
    {
        public string Action { get; private set; }
        public List<Tuple<string, string>> ArgList { get; private set; }

        public UPnPEventArgs(List<Tuple<string, string>> argList, string action)
        {
            ArgList = argList;
            Action = action;
        }
    }

    
}
