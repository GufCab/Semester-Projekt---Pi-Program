using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UPnP_Device.XML;

namespace UPnP_Device.UPnPConfig
{
    public interface IUPnPConfigPackage
    {
       // IXMLWriter XmlWr { get; }
        
        IIpConfig IpConf { get; }
        IUPnPConfig UpnpConf { get; }
        List<string> ServiceConfPaths { get; }
    }

    public class UPnPConfigPackage : IUPnPConfigPackage
    {
        //public IXMLWriter XmlWr { get; private set; }
        public IIpConfig IpConf { get; private set; }
        public IUPnPConfig UpnpConf { get; private set; }
        public List<string> ServiceConfPaths { get; private set; }
        
        public UPnPConfigPackage(IIpConfig ip, IUPnPConfig upnp, List<string>servicepath)
        {
            IpConf = ip;
            UpnpConf = upnp;
           // XmlWr = xw;
            ServiceConfPaths = servicepath;
        }
    }
}
