using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace UPnP_Device
{
    public class XMLWriter1
    {
        //DeviceArchitecture s.51
        //generates device XML
        public string genGETxml()
        {
            string path = @"Descriptions\desc.xml";
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
            udn.InnerText = "uuid:" + IPHandler.GetInstance().GUID;


            XmlElement serviceList = doc.CreateElement("serviceList");
            device.AppendChild(serviceList);

            XmlElement service = doc.CreateElement("service");
            serviceList.AppendChild(service);

            XmlElement serviceType = doc.CreateElement("serviceType");
            service.AppendChild(serviceType);
            serviceType.InnerText = "urn:schemas-upnp-org:service:AVTransport:1";

            XmlElement serviceId = doc.CreateElement("serviceId");
            service.AppendChild(serviceId);
            //serviceId.InnerText = "urn:upnp-org:serviceId:AVTransport.0001";
            serviceId.InnerText = "urn:upnp-org:serviceId:AVTransport";

            XmlElement SCPDURL = doc.CreateElement("SCPDURL");
            service.AppendChild(SCPDURL);
            //SCPDURL.InnerText = "urn-schemas-upnp-org-service-AVTransport.0001_scpd.xml";
           // SCPDURL.InnerText = "serviceDescripton.xml";
            SCPDURL.InnerText = "AVTransport/serviceDescription/";

            XmlElement controlURL = doc.CreateElement("controlURL");
            service.AppendChild(controlURL);
            //controlURL.InnerText = "urn:upnp-org:serviceId:AVTransport.0001_control";
            controlURL.InnerText = "";

            XmlElement eventSubUrl = doc.CreateElement("eventSubURL");
            service.AppendChild(eventSubUrl);
            eventSubUrl.InnerText = " ";
            
            //easy overview - for debugging
            doc.Save("DeviceDescDebug.xml");
            
            if (File.Exists(path))
                File.Delete(path);
            using (var fs = new FileStream(path, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(doc.OuterXml);
                sw.Close();
            }
           
            return doc.OuterXml;
        }

        //generates Service description XML
        public void genServiceXml()
        {
            #region setup

            string path = @"Descriptions\AVTransport\serviceDescription\desc.xml";
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

            #region play action

            XmlElement action = doc.CreateElement("action");
            actionList.AppendChild(action);

            XmlElement name = doc.CreateElement("name");
            action.AppendChild(name);
            name.InnerText = "Play";

            XmlElement argumentList = doc.CreateElement("argumentList");
            action.AppendChild(argumentList);

            XmlElement argument = doc.CreateElement("argument");
            argumentList.AppendChild(argument);

            XmlElement name_PlayArgument = doc.CreateElement("name");
            argument.AppendChild(name_PlayArgument);
            name_PlayArgument.InnerText = "InstanceID";

            XmlElement direction_play = doc.CreateElement("direction");
            argument.AppendChild(direction_play);
            direction_play.InnerText = "in";

            XmlElement relatedStateVariable_play = doc.CreateElement("relatedStateVariable");
            argument.AppendChild(relatedStateVariable_play);
            relatedStateVariable_play.InnerText = "A_ARG_TYPE_InstanceID";

            XmlElement argument1 = doc.CreateElement("argument");
            argumentList.AppendChild(argument1);

            XmlElement name_PlayArgumentSpeed = doc.CreateElement("name");
            argument1.AppendChild(name_PlayArgumentSpeed);
            name_PlayArgumentSpeed.InnerText = "Speed";

            XmlElement direction_playSpeed = doc.CreateElement("direction");
            argument1.AppendChild(direction_playSpeed);
            direction_playSpeed.InnerText = "in";

            XmlElement relatedStateVariable_playSpeed = doc.CreateElement("relatedStateVariable");
            argument1.AppendChild(relatedStateVariable_playSpeed);
            relatedStateVariable_playSpeed.InnerText = "TransportPlaySpeed";

            #endregion

            #region pause action

            action = doc.CreateElement("action");
            actionList.AppendChild(action);

            name = doc.CreateElement("name");
            action.AppendChild(name);
            name.InnerText = "Pause";

            argumentList = doc.CreateElement("argumentList");
            action.AppendChild(argumentList);

            argument = doc.CreateElement("argument");
            argumentList.AppendChild(argument);

            name_PlayArgument = doc.CreateElement("name");
            argument.AppendChild(name_PlayArgument);
            name_PlayArgument.InnerText = "InstanceID";

            direction_play = doc.CreateElement("direction");
            argument.AppendChild(direction_play);
            direction_play.InnerText = "in";

            relatedStateVariable_play = doc.CreateElement("relatedStateVariable");
            argument.AppendChild(relatedStateVariable_play);
            relatedStateVariable_play.InnerText = "A_ARG_TYPE_InstanceID";
            

            #endregion

            #region serviceStateTable

            XmlElement serviceStateTable = doc.CreateElement("serviceStateTable");
            scpd.AppendChild(serviceStateTable);
            
            XmlElement stateVariable = doc.CreateElement("stateVariable");
            serviceStateTable.AppendChild(stateVariable);
            //stateVariable.SetAttribute("sendEvents", "yes");

            XmlElement name_stateVariable = doc.CreateElement("name");
            stateVariable.AppendChild(name_stateVariable);
            name_stateVariable.InnerText = "A_ARG_TYPE_InstanceID";

            XmlElement sendEventAttribute = doc.CreateElement("sendEventAttribute");
            stateVariable.AppendChild(sendEventAttribute);
            sendEventAttribute.InnerText = "no";

            XmlElement dataType = doc.CreateElement("dataType");
            stateVariable.AppendChild(dataType);
            dataType.InnerText = "ui4";

            XmlElement stateVariableSpeed = doc.CreateElement("stateVariable");
            serviceStateTable.AppendChild(stateVariableSpeed);
            //stateVariable.SetAttribute("sendEvents", "yes");

            XmlElement name_stateVariableSpeed = doc.CreateElement("name");
            stateVariableSpeed.AppendChild(name_stateVariableSpeed);
            name_stateVariableSpeed.InnerText = "TransportPlaySpeed";

            XmlElement sendEventAttributeSpeed = doc.CreateElement("sendEventAttribute");
            stateVariableSpeed.AppendChild(sendEventAttributeSpeed);
            sendEventAttributeSpeed.InnerText = "no";

            XmlElement dataTypeSpeed = doc.CreateElement("dataType");
            stateVariableSpeed.AppendChild(dataTypeSpeed);
            dataTypeSpeed.InnerText = "string";

            #endregion

   


            doc.Save("ServiceXML.xml");

            if (File.Exists(path))
                File.Delete(path);
            using (var fs = new FileStream(path, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(doc.OuterXml);
                sw.Close();
            }
        }
    }


}
