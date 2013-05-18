using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device.UPnPConfig;
using UPnP_Device;
using UPnP_Device.XML;

namespace UPnPConfigFactory
{
    public interface IUPnPConfigFactory
    {
        IUPnPConfigPackage CreatePackage();
    }

    public class SinkFactory : IUPnPConfigFactory
    {


        public IUPnPConfigPackage CreatePackage()
        {
            IIpConfig ip = new SinkIPConfig(52000);
            IUPnPConfig upnp = new UPnPConfig(900, "config/ConfigHiPiMediaRenderer.txt");
            IXMLWriter xml = new XMLWriter(ip);

            List<string> serviceconf = new List<string>
                {
                    "config/ConfigServiceAVTransport.txt",
                    "config/ConfigServiceRenderingControl.txt"
                };

            IUPnPConfigPackage pack = new UPnPConfigPackage(ip, upnp, xml, serviceconf);

            return pack;
        }
    }

    /*
    public class SourceFactory : IUPnPConfigFactory
    {


        public IUPnPConfigPackage CreatePackage()
        {
            IUPnPConfigPackage pack = new UPnPConfigPackage();



            return pack;
        }
    }
     * */
}
