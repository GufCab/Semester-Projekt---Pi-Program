using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UPnP_Device.TCP;
using UPnP_Device.UDP;
using UPnP_Device.UPnP;

//Todo: Looks VERY confusing. Refactoring needed???

namespace UPnP_Device
{
    public interface IUPnPMain
    {
        
    }

    public class XMLWriter1
    {
        public void genDeviceDescription()
        {
            
        }
        public void genServiceXmlAVTransport()
        {
            
        }
    }

    public class UPnPMain : IUPnPMain
    {
        //private TCPReceiver tcpReceiver;
        private UDPHandler _udpHandler;

        private const int cacheExpire = 1800; //Cache expire in seconds
        private const int port = 52000;

        public string UUID { get; private set; }
        public string localIP { get; private set; }

        public delegate void UPnPEventDel(object e, List<Tuple<string, string>> args, string action);

        public event UPnPEventDel UPnPEvent = delegate{};
        private TcpServer server;
        private EventHandler eventHandler;

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

            _udpHandler = new UDPHandler(UUID, cacheExpire, localIP, port);
            _udpHandler.Start();

            SubscribeToUpnpEvents();
        }    

        private void SubscribeToUpnpEvents()
        {
            eventHandler.UPnPEvent += eventHandler_UPnPEvent;
        }

        void eventHandler_UPnPEvent(object e, List<Tuple<string, string>> args, string action)
        {
            //Todo: Raise event..
        }

    }
}
