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
    /// <summary>
    /// Interface for all UPnP Devices
    /// </summary>
    public interface IUPnP
    {
        event ActionEventDel ActionEvent;
		string GetIP();
        
    }

    /// <summary>
    /// Used for ActionEvents
    /// </summary>
    public class EventClass
    {
       public  delegate void NewActionEventDel(object e, UPnPEventArgs args, CallBack cb);
    }

    /// <summary>
    /// UPnP Device class
    /// </summary>
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

        /// <summary>
        /// Constructor
        /// Configures UPnP Device with UPnP config package
        /// </summary>
        /// <param name="config">Configuration package</param>
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

        /// <summary>
        /// Subscribe to TcpAction event
        /// </summary>
        private void SubscribeToUpnpEvents()
        {
            EventContainer.TcpActionEvent += EventHandler_UPnPEvent;
        }

        /// <summary>
        /// Handler function for TcpActionEvent
        /// Raises ActionEvent.
        /// </summary>
        /// <param name="e">Sender</param>
        /// <param name="args">UPnP arguments received over TCP</param>
        /// <param name="callBack">Function to be called to</param>
        void EventHandler_UPnPEvent(object e, UPnPEventArgs args, CallBack callBack)
        {
            
                ActionEvent(e, args, callBack);
        }

        /// <summary>
        /// Get IP adress from the configurations package.
        /// </summary>
        /// <returns></returns>
		public string GetIP()
		{
			return IpConf.IP;
		}
    }
}
