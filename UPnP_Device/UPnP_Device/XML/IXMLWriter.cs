using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device.XML
{
    public interface IXMLWriter
    {
        void GenDeviceDescription();
        void GenServiceDescription();
    }
}
