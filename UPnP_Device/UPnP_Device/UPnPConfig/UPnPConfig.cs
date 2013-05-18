using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UPnP_Device.UPnPConfig
{
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

        void LoadConfig(string path);
    }
    
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
        
        //Todo: Contructor should take some NT's in some way
        public UPnPConfig(string path)
        {
            services = new List<string>();
            cacheExpire = 30 * 60 * 1000;

            //GenerateNT();
            LoadConfig(path);
        }
        
        public UPnPConfig(int expireTime, string path)
        {
            services = new List<string>();
            cacheExpire = expireTime * 1000;

            //GenerateNT();
            LoadConfig(path);
        }
        
        public void LoadConfig(string path)
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

        private void SetProperties(List<string> configInfo)
        {
            friendlyName = configInfo[0];
            //DeviceSchema = configInfo[1];
            Manufacturer = configInfo[1];
            ModelName = configInfo[2];
            ModelDescription = configInfo[3];
            ManufacturerURL = configInfo[4];
            DeviceType = configInfo[5];
            
            configInfo.RemoveRange(0, 6);

            foreach (string s in configInfo)
            {
                services.Add(s);
            }

            GenerateNT();
        }
        
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
