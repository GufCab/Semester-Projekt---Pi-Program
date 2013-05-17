using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device.UPnPConfig;
using UPnP_Device.XML;

namespace UPnP_Device
{
    public interface IConfigInjector
    {
        UPnPConfigPackage GetUPnPConfigPackage();
    }

    public class ConfigInjectorSink : IConfigInjector
    {
        public UPnPConfigPackage GetUPnPConfigPackage()
        {
            IXMLWriter XmlWriterSink = new XMLWriterSink();
            IIpConfig IPConfig = new SinkIPConfig(52000);
            IUPnPConfig UPnPConfig = new UPnPConfig.UPnPConfig(1800);
            
            return new UPnPConfigPackage(IPConfig, UPnPConfig, XmlWriterSink);
        }
    }
    /*
    public class ConfigInjectorSource : IConfigInjector
    {
        public UPnPConfigPackage GetUPnPConfigPackage()
        {
            IXMLWriter XmlWriterSink = new XMLWriterSink();
            IIpConfig IPConfig = new SourceIPConfig(52001);
            IUPnPConfig UPnPConfig = new SourceUPnPConfig(1800);

            return new UPnPConfigPackage(IPConfig, UPnPConfig, XmlWriterSink);
        }
    }*/
}
