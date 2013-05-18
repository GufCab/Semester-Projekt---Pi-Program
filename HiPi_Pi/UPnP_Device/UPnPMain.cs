using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UPnP_Device.TCP;
using UPnP_Device.UDP;

//Todo: Looks VERY confusing. Refactoring needed???

/*
namespace UPnP_Device
{
    public interface IUPnPMain
    {
        event ActionEventDel ActionEvent;
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

        public event ActionEventDel ActionEvent;


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

            //Todo: Remember to create a new EventHandler:
            eventHandler = new EventHandler();
        }    

        private void SubscribeToUpnpEvents()
        {
            EventContainer.TcpActionEvent += EventHandler_UPnPEvent;
        }

        void EventHandler_UPnPEvent(object e, UPnPEventArgs args, CallBack callBack)
        {
            ActionEvent(e, args, callBack);
        }

    }
}
*/