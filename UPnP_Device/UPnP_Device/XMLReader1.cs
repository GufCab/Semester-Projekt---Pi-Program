 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UPnP_Device
{
    public class XMLReader1
    {
        public string loadAVTransportXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"\XML_files");

            return doc.OuterXml;
        }
    }
}
