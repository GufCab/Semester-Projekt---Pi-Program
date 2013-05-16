﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UPnP_Device.UPnP;

namespace UPnP_Device.UPnPConfig
{
    public interface IXMLWriter
    {
        
    }

    public class UPnPConfigPackage
    {
        public IXMLWriter xmlwr { get; private set; }
        public IIpConfig ipconf { get; private set; }
        public IUPnPConfig upnpconf { get; private set; }
        public IEventConfig events { get; private set; }

        public UPnPConfigPackage(IIpConfig ip, IUPnPConfig upnp, IEventConfig ev, IXMLWriter xw)
        {
            ipconf = ip;
            upnpconf = upnp;
            events = ev;
            xmlwr = xw;
        }
    }
}
