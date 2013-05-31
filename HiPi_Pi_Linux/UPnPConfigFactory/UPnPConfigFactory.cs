using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device.UPnPConfig;
using UPnP_Device;
using UPnP_Device.XML;

/// <summary>
/// Namespace for all configuration factory-related members.
/// </summary>
namespace UPnPConfigFactory
{
    /// <summary>
    /// Interface for UPnP configuration factories
    /// Creates the package sent to devices.
    /// </summary>
    public interface IUPnPConfigFactory
    {
        IUPnPConfigPackage CreatePackage();
    }

    /// <summary>
    /// Concrete factory implementing the IUPnPConfigFactory.
    /// This factory is used for creating the config package for Sink devices.
    /// </summary>
    public class SinkFactory : IUPnPConfigFactory
    {
        /// <summary>
        /// Function creating the package
        /// 
        /// Reads configuration files and loads them into the package. 
        /// </summary>
        /// <returns>Configuration package</returns>
        public IUPnPConfigPackage CreatePackage()
        {
            IIpConfig ip = new SinkIPConfig(52200);
            IUPnPConfig upnp = new UPnPConfig(900, "../../config/ConfigHiPiMediaRenderer.txt");
            
           // IXMLWriter xml = new XMLWriter(ip, upnp);

            List<string> serviceconf = new List<string>
                {
                    "../../config/ConfigServiceAVTransport.txt",
                    "../../config/ConfigServiceRenderingControl.txt",
					"../../config/ConfigServiceConnectionManager.txt"
                };

            IUPnPConfigPackage pack = new UPnPConfigPackage(ip, upnp, serviceconf);

            return pack;
        }
    }

    /// <summary>
    /// Concrete factory implementing the IUPnPConfigFactory.
    /// This factory is used for creating the config package for Sink devices.
    /// </summary>
    public class SourceFactory : IUPnPConfigFactory
    {
        /// <summary>
        /// Function creating the package
        /// 
        /// Reads configuration files and loads them into the package. 
        /// </summary>
        /// <returns>Configuration package</returns>
        public IUPnPConfigPackage CreatePackage()
        {
            SourceIPConfig ipConf = new SourceIPConfig(52100);
 
            IUPnPConfig upnp = new UPnPConfig(900, "../../config/ConfigHiPiMediaServer.txt");

            List<string> serviceconf = new List<string>
                {
                    "../../config/ConfigServiceContentDirectory.txt",
					"../../config/ConfigServiceConnectionManager.txt"
                };

            IUPnPConfigPackage pack = new UPnPConfigPackage(ipConf,upnp,serviceconf);
            return pack;
        }
    }

}
