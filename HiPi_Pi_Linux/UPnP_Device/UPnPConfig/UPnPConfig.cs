using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UPnP_Device.UPnPConfig
{
    /// <summary>
    /// Interface used to configure UPnP device
    /// with device related attributes.
    /// </summary>
    public interface IUPnPConfig
    {
        List<string> NT { get; }
        int cacheExpire { get; }

        //UPnP description details:
        List<string> services { get; set; }
        string friendlyName { get; set; }
        //string DeviceSchema { get; set; }
        string Manufacturer { get; set; }
        string ModelName { get; set; }
        string ModelDescription { get; set; }
        string ManufacturerURL { get; set; }
        string DeviceType { get; set; }

        string BasePath { get; set; }
    }
    
    /// <summary>
    /// Implements the interface IUPnPConfig.
    /// 
    /// Created by the UPnP creator factory.
    /// See UPnP documentation for description of all values in this.
    /// 
    /// Values are loaded in from description files. 
    /// </summary>
    public class UPnPConfig : IUPnPConfig
    {
        public List<string> NT { get; private set; }
        public int cacheExpire { get; private set; }

        //UPnP description details:
        public List<string> services { get; set; }
        public string friendlyName { get; set; }
        //public string DeviceSchema { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public string ManufacturerURL { get; set; }
        public string DeviceType { get; set; }

        public string BasePath { get; set; }
        
        //Todo: Contructor should take some NT's in some way
        /// <summary>
        /// Constructor.
        /// Sets cacheExpire to default value.
        /// </summary>
        /// <param name="path">Path to configuration file</param>
        public UPnPConfig(string path)
        {
            services = new List<string>();
            cacheExpire = 30 * 60 * 1000;

            //GenerateNT();
            LoadConfig(path);
        }
        
        /// <summary>
        /// Overloaded constructor.
        /// Creates instance with the expire value, that is sent as argument.
        /// </summary>
        /// <param name="expireTime">Time until new broadcast of Notify messages</param>
        /// <param name="path">Path to configuration file</param>
        public UPnPConfig(int expireTime, string path)
        {
            services = new List<string>();
            cacheExpire = expireTime * 1000;

            //GenerateNT();
            LoadConfig(path);
        }
        
        /// <summary>
        /// Load configurations in to a List of strings.
        /// </summary>
        /// <param name="path"></param>
        private void LoadConfig(string path)
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                List<string> lines = new List<string>();
                string line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                SetProperties(lines);
            }
        }

        /// <summary>
        /// Set values of properties to values read from config list.
        /// </summary>
        /// <param name="configInfo">Info read from file</param>
        private void SetProperties(List<string> configInfo)
        {
            friendlyName = configInfo[0];
            BasePath = configInfo[1];
            Manufacturer = configInfo[2];
            ModelName = configInfo[3];
            ModelDescription = configInfo[4];
            ManufacturerURL = configInfo[5];
            DeviceType = configInfo[6];
            
            configInfo.RemoveRange(0, 7);

            foreach (string s in configInfo)
            {
                services.Add(s);
            }

            GenerateNT();
        }
        
        /// <summary>
        /// Generates NT for UDP multicast.
        /// </summary>
        private void GenerateNT()
        {
            NT = new List<string>();
            NT.Add("upnp:rootdevice");
            NT.Add(DeviceType);

            foreach (string s in services)
            {
                NT.Add("urn:schemas-upnp-org:service:" + s);
            }
        }
    }

    /*
    //Todo: check that everything is implemented as above
    public class SourceUPnPConfig : IUPnPConfig
    {
        public List<string> NT { get; private set; }
        public List<string> services { get; set; }

        public int cacheExpire { get; private set; }

        public string friendlyName { get; set; }
        public string DeviceSchema { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
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
            services = new List<string>();
            services.Add("ContentDirectory:1");
            
            DeviceType = "urn:schemas-upnp-org:device:MediaServer:1";

            NT = new List<string>();
            NT.Add("upnp:rootdevice");
            NT.Add(DeviceType);
            foreach (string s in services)
            {
                NT.Add("urn:schemas-upnp-org:service:" + s);
            }

            DeviceSchema = "urn:schemas-upnp-org:device::";
            friendlyName = "HiPi_Source";
        }
    }*/
}
