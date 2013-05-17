using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device.UPnPConfig;

namespace UPnP_Device.XML
{
    public interface IXMLWriter
    {
        void GenDeviceDescription(IUPnPConfig UPnPConfig);
        void GenServiceDescription();
    }
}
