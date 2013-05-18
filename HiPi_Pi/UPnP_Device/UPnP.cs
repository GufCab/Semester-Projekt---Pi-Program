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
    }

    public class UPnP : IUPnP
    {
        private IIpConfig IpConf;
        private IUPnPConfig UpnpConf;
        private IXMLWriter XmlWriter;

        private TcpServer TCPServer;
        private UDPHandler UDPServer;

        public event ActionEventDel ActionEvent = delegate { };

        public UPnP(IUPnPConfigPackage config)
        {
            IpConf = config.IpConf;
            UpnpConf = config.UpnpConf;
            
            XmlWriter = new XMLWriter(IpConf, UpnpConf);
            XmlWriter.GenDeviceDescription();

            XMLServicesConfig servicesConfig = new XMLServicesConfig(config.ServiceConfPaths, XmlWriter);
            
            TCPServer = new TcpServer(IpConf.IP, IpConf.TCPPort, UpnpConf.BasePath);
            UDPServer = new UDPHandler(IpConf, UpnpConf);
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
    }
}
