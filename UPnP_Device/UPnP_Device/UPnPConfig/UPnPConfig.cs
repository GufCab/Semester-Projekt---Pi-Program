using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device.UPnPConfig
{
    public interface IUPnPConfig
    {
        List<string> NT { get; }
        int cacheExpire { get; }

        //UPnP description details:
        string friendlyName { get; set; }
        string DeviceSchema { get; set; }
        //Todo: Add other upnp info used in XML
    }

    public class SinkUPnPConfig : IUPnPConfig
    {
        public List<string> NT { get; private set; }

        public int cacheExpire { get; private set; }

        public string friendlyName { get; set; }
        public string DeviceSchema { get; set; }

        //Todo: Contructor should take some NT's in some way
        public SinkUPnPConfig()
        {
            cacheExpire = 30 * 1000;
            contruct();
        }
        
        public SinkUPnPConfig(int expireTime)
        {
            cacheExpire = expireTime * 1000;
            contruct();
        }

        //Everything that should be done in all the overloads of the contructor
        private void contruct()
        {
            NT = new List<string>();
            NT.Add("upnp:rootdevice");
            NT.Add("urn:schemas-upnp-org:device:MediaRenderer:1");
            NT.Add("urn:schemas-upnp-org:service:AVTransport:1");
            NT.Add("urn:schemas-upnp-org:service:RenderingControl:1");
            
            DeviceSchema = "urn:schemas-upnp-org:device::";
        }
    }
}
