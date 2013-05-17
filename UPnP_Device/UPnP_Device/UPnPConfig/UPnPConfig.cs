using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device.UPnPConfig
{
    public interface IUPnPConfig
    {
        List<string> NT { get; }
        List<string> services { get; }
        int cacheExpire { get; }

        //UPnP description details:
        string friendlyName { get; set; }
        string DeviceSchema { get; set; }
        string Manufacturer { get; set; }
        string ModelName { get; set; }
        string ModelDesc { get; set; }
        string ManufacturerURL { get; set; }
        string DeviceType { get; set; }
        //Todo: Add other upnp info used in XML
    }
    
    public class SinkUPnPConfig : IUPnPConfig
    {
        public List<string> NT { get; private set; }

        public int cacheExpire { get; private set; }

        public string friendlyName { get; set; }
        public string DeviceSchema { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string ModelDesc { get; set; }
        public string ManufacturerURL { get; set; }
        public string DeviceType { get; set; }

        public List<string> services { get; }

        //Todo: Contructor should take some NT's in some way
        public SinkUPnPConfig()
        {
            cacheExpire = 30 * 60 * 1000;
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
            services = new List<string>();
            services.Add("AVTransport:1");
            services.Add("RenderingControl:1");

            DeviceType = "urn:schemas-upnp-org:device:MediaRenderer:1";
            
            NT = new List<string>();
            NT.Add("upnp:rootdevice");
            NT.Add(DeviceType);
            foreach (string s in services)
            {
                NT.Add("urn:schemas-upnp-org:service:" + s);
            }

            DeviceSchema = "urn:schemas-upnp-org:device::";
            friendlyName = "HiPi_Sink";
        }
    }

    public class SourceUPnPConfig : IUPnPConfig
    {
        public List<string> NT { get; private set; }

        public int cacheExpire { get; private set; }

        public string friendlyName { get; set; }
        public string DeviceSchema { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string ModelDesc { get; set; }
        public string ManufacturerURL { get; set; }
        public string DeviceType { get; set; }

        //Todo: Contructor should take some NT's in some way
        public SourceUPnPConfig()
        {
            cacheExpire = 30 * 60 * 1000;
            contruct();
        }

        public SourceUPnPConfig(int expireTime)
        {
            cacheExpire = expireTime * 1000;
            contruct();
        }

        //Everything that should be done in all the overloads of the contructor
        private void contruct()
        {
            DeviceType = "urn:schemas-upnp-org:device:MediaServer:1"; 
            NT = new List<string>();
            NT.Add("upnp:rootdevice");
            NT.Add(DeviceType);
            NT.Add("urn:schemas-upnp-org:service:ContentDirectory:1");

            DeviceSchema = "urn:schemas-upnp-org:device::";
            friendlyName = "HiPi";
        }
    }
}
