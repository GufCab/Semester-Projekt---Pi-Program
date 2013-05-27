using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device.TCP;
using UPnP_Device.UDP;
using UPnP_Device.UPnPConfig;
using UPnP_Device.XML;

namespace UPnP_Device
{
    public interface IUPnP
    {
        event ActionEventDel ActionEvent;
		string GetIP();
        
    }

    public class EventClass
    {
       public  delegate void NewActionEventDel(object e, UPnPEventArgs args, CallBack cb);
    }

    public class UPnP : IUPnP
    {
        private IIpConfig IpConf;
        private IUPnPConfig UpnpConf;
        private IXMLWriter XmlWriter;

        private TcpServer TCPServer;
        private UDPServer UDPServer;

        public EventClass evt = new EventClass();

        public event EventClass.NewActionEventDel myEvent;
        
        public event ActionEventDel ActionEvent = delegate { };

        public UPnP(IUPnPConfigPackage config)
        {
            IpConf = config.IpConf;
            UpnpConf = config.UpnpConf;
            
            XmlWriter = new XMLWriter(IpConf, UpnpConf);
            XmlWriter.GenDeviceDescription();

            XMLServicesConfig servicesConfig = new XMLServicesConfig(config.ServiceConfPaths, XmlWriter);
            
            TCPServer = new TcpServer(IpConf, UpnpConf.BasePath);
            UDPServer = new UDPServer(IpConf, UpnpConf);
            UDPServer.Start();

            SubscribeToUpnpEvents();



        }

        private void SubscribeToUpnpEvents()
        {
            EventContainer.TcpActionEvent += EventHandler_UPnPEvent;
        }

        void EventHandler_UPnPEvent(object e, UPnPEventArgs args, CallBack callBack)
        {
            
                ActionEvent(e, args, callBack);
        }

		public string GetIP()
		{
			return IpConf.IP;
		}
    }
}
