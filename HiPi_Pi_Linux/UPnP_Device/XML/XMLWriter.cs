using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UPnP_Device.UPnPConfig;

namespace UPnP_Device.XML
{
    public interface IXMLWriter
    {
        void GenDeviceDescription();
        void GenServiceDescription(string type, List<FunctionProperties> functions);
    }

    public class XMLWriter : IXMLWriter
    {
        //public string descriptionsPath;
        public string filename = "desc.xml";
        public string AVTransportServicePath = @"AVTransport/serviceDescription/";
        public string RenderingControlServicePath = @"RenderingControl/serviceDescription/";
        //public string servicePath = "serviceDescription/";
        private IIpConfig _ip;
        private IUPnPConfig _upnp;

        //public XMLWriter(){}

        public XMLWriter(IIpConfig ip, IUPnPConfig upnp)
        {
            _ip = ip;
            _upnp = upnp;
        }

        //DeviceArchitecture s.51
        //generates device XML
        public void GenDeviceDescription()
        {
            #region setup

            //string path = @"Descriptions\desc.xml";
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(root);
            root.SetAttribute("xmlns", "urn:schemas-upnp-org:device-1-0");

            XmlElement specVersion = doc.CreateElement("specVersion");
            root.AppendChild(specVersion);

            XmlElement major = doc.CreateElement("major");
            specVersion.AppendChild(major);
            major.InnerText = "1";

            XmlElement minor = doc.CreateElement("minor");
            specVersion.AppendChild(minor);
            minor.InnerText = "0";

            XmlElement device = doc.CreateElement("device");
            root.AppendChild(device);

            #endregion

            #region mediaRenderer

            XmlElement deviceType = doc.CreateElement("deviceType");
            device.AppendChild(deviceType);
            deviceType.InnerText = _upnp.DeviceType;

            XmlElement friendlyName = doc.CreateElement("friendlyName");
            device.AppendChild(friendlyName);
            friendlyName.InnerText = _upnp.friendlyName;

            XmlElement manufacturer = doc.CreateElement("manufacturer");
            device.AppendChild(manufacturer);
            manufacturer.InnerText = _upnp.Manufacturer;

            XmlElement manufacturerURL = doc.CreateElement("manufacturerURL");
            device.AppendChild(manufacturerURL);
            manufacturerURL.InnerText = _upnp.ManufacturerURL;

            XmlElement modelDescription = doc.CreateElement("modelDescription");
            device.AppendChild(modelDescription);
            modelDescription.InnerText = _upnp.ModelDescription;

            XmlElement modelName = doc.CreateElement("modelName");
            device.AppendChild(modelName);
            modelName.InnerText = _upnp.ModelName;

            XmlElement udn = doc.CreateElement("UDN");
            device.AppendChild(udn);
            udn.InnerText = "uuid:" + _ip.GUID;


            XmlElement serviceList = doc.CreateElement("serviceList");
            device.AppendChild(serviceList);

            #endregion

            #region AVTransport



            foreach (string s in _upnp.services)
            {
                XmlElement service = doc.CreateElement("service");
                serviceList.AppendChild(service);

                XmlElement serviceType = doc.CreateElement("serviceType");
                service.AppendChild(serviceType);
                serviceType.InnerText = "urn:schemas-upnp-org:service:" + s;

                XmlElement serviceId = doc.CreateElement("serviceId");
                service.AppendChild(serviceId);
                //serviceId.InnerText = "urn:upnp-org:serviceId:AVTransport.0001";
                serviceId.InnerText = "urn:upnp-org:serviceId:" + s.Split(':')[0];

                XmlElement SCPDURL = doc.CreateElement("SCPDURL");
                service.AppendChild(SCPDURL);
                //SCPDURL.InnerText = "urn-schemas-upnp-org-service-AVTransport.0001_scpd.xml";
                // SCPDURL.InnerText = "serviceDescripton.xml";
                SCPDURL.InnerText = s.Split(':')[0];

                XmlElement controlURL = doc.CreateElement("controlURL");
                service.AppendChild(controlURL);
                //controlURL.InnerText = "urn:upnp-org:serviceId:AVTransport.0001_control";
                controlURL.InnerText = "";

                XmlElement eventSubUrl = doc.CreateElement("eventSubURL");
                service.AppendChild(eventSubUrl);
                eventSubUrl.InnerText = "";
            }

            #endregion
            
            //for debug
            doc.Save("DeviceDescDebug.xml");
            
            SaveFile(doc.OuterXml ,"");
        }

        public void SaveFile(string xml, string servicePath)
        {
            string[] s = servicePath.Split(':');

            if (Directory.Exists(_upnp.BasePath + "/" + s[0]) == false)
            {
                Directory.CreateDirectory(_upnp.BasePath + "/" + s[0]);
            }
            using (var fs = new FileStream(_upnp.BasePath + "/" + s[0] + "/" + filename, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(xml);
                sw.Close();
            }
        }

        public void GenServiceDescription(string type, List<FunctionProperties> functions)
        {
            #region setup
            
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement scpd = doc.CreateElement("scpd");
            doc.AppendChild(scpd);
            scpd.SetAttribute("xmlns", "urn:schemas-upnp-org:service-1-0");

            XmlElement specVersion = doc.CreateElement("specVersion");
            scpd.AppendChild(specVersion);

            XmlElement major = doc.CreateElement("major");
            specVersion.AppendChild(major);
            major.InnerText = "1";

            XmlElement minor = doc.CreateElement("minor");
            specVersion.AppendChild(minor);
            minor.InnerText = "0";

            XmlElement actionList = doc.CreateElement("actionList");
            scpd.AppendChild(actionList);

            #endregion

            #region actions

            foreach (FunctionProperties functionPropertie in functions)
            {
                XmlElement action = doc.CreateElement("action");
                actionList.AppendChild(action);

                XmlElement name = doc.CreateElement("name");
                action.AppendChild(name);
                name.InnerText = functionPropertie.functionName;

                XmlElement argumentList = doc.CreateElement("argumentList");
                action.AppendChild(argumentList);
                
                foreach (var arg in functionPropertie.arguments)
                {
                    XmlElement argument = doc.CreateElement("argument");
                    argumentList.AppendChild(argument);

                    XmlElement name_PlayArgument = doc.CreateElement("name");
                    argument.AppendChild(name_PlayArgument);
                    name_PlayArgument.InnerText = arg.argumentName;

                    XmlElement direction_play = doc.CreateElement("direction");
                    argument.AppendChild(direction_play);
                    direction_play.InnerText = arg.direction;

                    XmlElement relatedStateVariable_play = doc.CreateElement("relatedStateVariable");
                    argument.AppendChild(relatedStateVariable_play);
                    relatedStateVariable_play.InnerText = arg.relatedStateVariable;
                }
            }

            #endregion

            #region relatedStateVariables

            List<ArgumentProperties> argumentPropertie = new List<ArgumentProperties>();

            foreach (FunctionProperties functionPropertie in functions)
            {
                argumentPropertie.AddRange(functionPropertie.arguments);
            }

            var stateVariablesDistinct = argumentPropertie.GroupBy(p => p.argumentName).Select(grp => grp.First()).ToList();

            XmlElement serviceStateTable = doc.CreateElement("serviceStateTable");
            scpd.AppendChild(serviceStateTable);

            foreach (ArgumentProperties arg in stateVariablesDistinct)
            {
                    XmlElement stateVariable = doc.CreateElement("stateVariable");
                    serviceStateTable.AppendChild(stateVariable);
                    stateVariable.SetAttribute("sendEvents", arg.sendEventAttribute);

                    XmlElement name_stateVariable = doc.CreateElement("name");
                    stateVariable.AppendChild(name_stateVariable);
                    name_stateVariable.InnerText = arg.relatedStateVariable;

					/*
                    XmlElement sendEventAttribute = doc.CreateElement("sendEventAttribute");
                    stateVariable.AppendChild(sendEventAttribute);
                    sendEventAttribute.InnerText = arg.sendEventAttribute;
                    */

                    XmlElement dataType = doc.CreateElement("dataType");
                    stateVariable.AppendChild(dataType);
                    dataType.InnerText = arg.dataType;
            }

            #endregion

            //only for debug
            //doc.Save(type + "Service.xml");

            SaveFile(doc.OuterXml, type);
        }
    }
}
