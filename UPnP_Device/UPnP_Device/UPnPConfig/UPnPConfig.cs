﻿using System;
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
        public SinkUPnPConfig(int expireTime)
        {
            NT = new List<string>();
            NT.Add("upnp:rootdevice");      //Todo: Should probably be some Sink stuff

            cacheExpire = expireTime;

            DeviceSchema = "urn:schemas-upnp-org:device::";
        }
    }
}
