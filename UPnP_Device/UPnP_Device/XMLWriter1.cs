using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UPnP_Device
{
    public class XMLWriter1
    {
        //DeviceArchitecture s.51
        public string genGETxml()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(root);
            root.SetAttribute("xmlns", "urn:schemas-upnp-org:device-1-0");
            root.SetAttribute("configId", "1");

            XmlElement specVersion = doc.CreateElement("specVersion");
            root.AppendChild(specVersion);

            XmlElement major = doc.CreateElement("major");
            specVersion.AppendChild(major);
            major.InnerText = "1";

            XmlElement minor = doc.CreateElement("minor");
            specVersion.AppendChild(minor);
            minor.InnerText = "1";

            XmlElement device = doc.CreateElement("device");
            root.AppendChild(device);

            XmlElement deviceType = doc.CreateElement("deviceType");
            device.AppendChild(deviceType);
            deviceType.InnerText = "urn:schemas-upnp-org:device:MediaRenderer:1";

            XmlElement friendlyName = doc.CreateElement("friendlyName");
            device.AppendChild(friendlyName);
            friendlyName.InnerText = "HiPi";

            //XmlElement presentationURL = doc.CreateElement("presentationURL");
            //device.AppendChild(presentationURL);
            //presentationURL.InnerText = " ";

            XmlElement manufacturer = doc.CreateElement("manufacturer");
            device.AppendChild(manufacturer);
            manufacturer.InnerText = "Gruppe 8";

            XmlElement manufacturerURL = doc.CreateElement("manufacturerURL");
            device.AppendChild(manufacturerURL);
            manufacturerURL.InnerText = " ";

            XmlElement modelDescription = doc.CreateElement("modelDescription");
            device.AppendChild(modelDescription);
            modelDescription.InnerText = "Social Soundsystem";

            XmlElement modelName = doc.CreateElement("modelName");
            device.AppendChild(modelName);
            modelName.InnerText = "HiPi";

            XmlElement udn = doc.CreateElement("UDN");
            device.AppendChild(udn);
            udn.InnerText = IPHandler.GetInstance().GUID;

            XmlElement iconList = doc.CreateElement("iconList");
            device.AppendChild(iconList);

            XmlElement serviceList = doc.CreateElement("serviceList");
            device.AppendChild(serviceList);

            XmlElement service = doc.CreateElement("service");
            serviceList.AppendChild(service);

            XmlElement serviceType = doc.CreateElement("serviceType");
            service.AppendChild(serviceType);
            serviceType.InnerText = "urn:schemas-upnp-org:service:AVTransport:1";

            XmlElement serviceId = doc.CreateElement("serviceId");
            service.AppendChild(serviceId);
            serviceId.InnerText = "urn:upnp-org:serviceId:AVTransport.0001";

            XmlElement SCPDURL = doc.CreateElement("SCPDURL");
            service.AppendChild(SCPDURL);

            //SCPDURL.InnerText = "urn-schemas-upnp-org-service-AVTransport.0001_scpd.xml";
            SCPDURL.InnerText = "test";

            XmlElement controlURL = doc.CreateElement("controlURL");
            service.AppendChild(controlURL);
            controlURL.InnerText = "urn:upnp-org:serviceId:AVTransport.0001_control";

            XmlElement eventSubUrl = doc.CreateElement("eventSubURL");
            service.AppendChild(eventSubUrl);
            eventSubUrl.InnerText = "urn:upnp-org:serviceId:AVTransport.0001_event";

            doc.Save("NewDesc.xml");

            return doc.OuterXml;
        }
    }


}
