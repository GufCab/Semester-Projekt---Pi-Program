using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UPnP_Device.UPnP;
using UPnP_Device.UPnPConfig;

namespace UPnP_Device.XML
{
    public interface IXMLWriter
    {
        void GenDeviceDescription(IUPnPConfig UPnPConfig);
        void GenServiceDescription(string type, List<FunctionProperties> functions);
    }

    public class XMLWriter : IXMLWriter
    {
        public string descriptionsPath = @"Descriptions/";
        public string filename = "desc.xml";
        public string AVTransportServicePath = @"AVTransport/serviceDescription/";
        public string RenderingControlServicePath = @"RenderingControl/serviceDescription/";
        public string servicePath = "/serviceDescription/";

        //DeviceArchitecture s.51
        //generates device XML
        public void GenDeviceDescription(IUPnPConfig UPnPConfig)
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
            deviceType.InnerText = UPnPConfig.DeviceType;

            XmlElement friendlyName = doc.CreateElement("friendlyName");
            device.AppendChild(friendlyName);
            friendlyName.InnerText = UPnPConfig.friendlyName;

            XmlElement manufacturer = doc.CreateElement("manufacturer");
            device.AppendChild(manufacturer);
            manufacturer.InnerText = UPnPConfig.Manufacturer;

            XmlElement manufacturerURL = doc.CreateElement("manufacturerURL");
            device.AppendChild(manufacturerURL);
            manufacturerURL.InnerText = UPnPConfig.ManufacturerURL;

            XmlElement modelDescription = doc.CreateElement("modelDescription");
            device.AppendChild(modelDescription);
            modelDescription.InnerText = UPnPConfig.ModelDescription;

            XmlElement modelName = doc.CreateElement("modelName");
            device.AppendChild(modelName);
            modelName.InnerText = UPnPConfig.ModelName;

            XmlElement udn = doc.CreateElement("UDN");
            device.AppendChild(udn);
            udn.InnerText = "uuid:" + IPHandler.GetInstance().GUID;


            XmlElement serviceList = doc.CreateElement("serviceList");
            device.AppendChild(serviceList);

            #endregion

            #region AVTransport

            XmlElement service = doc.CreateElement("service");
            serviceList.AppendChild(service);

            foreach (string s in UPnPConfig.services)
            {
                XmlElement serviceType = doc.CreateElement("serviceType");
                service.AppendChild(serviceType);
                serviceType.InnerText = "urn:schemas-upnp-org:service:" + s;

                XmlElement serviceId = doc.CreateElement("serviceId");
                service.AppendChild(serviceId);
                //serviceId.InnerText = "urn:upnp-org:serviceId:AVTransport.0001";
                serviceId.InnerText = "urn:upnp-org:serviceId:" + s;

                XmlElement SCPDURL = doc.CreateElement("SCPDURL");
                service.AppendChild(SCPDURL);
                //SCPDURL.InnerText = "urn-schemas-upnp-org-service-AVTransport.0001_scpd.xml";
                // SCPDURL.InnerText = "serviceDescripton.xml";
                SCPDURL.InnerText = s.Split(':')[0] + servicePath;

                XmlElement controlURL = doc.CreateElement("controlURL");
                service.AppendChild(controlURL);
                //controlURL.InnerText = "urn:upnp-org:serviceId:AVTransport.0001_control";
                controlURL.InnerText = "";

                XmlElement eventSubUrl = doc.CreateElement("eventSubURL");
                service.AppendChild(eventSubUrl);
                eventSubUrl.InnerText = "";
            }

            #endregion
            
            //ATTENTION: Outcommented:
            #region RenderingControl
            /*
            service = doc.CreateElement("service");
            serviceList.AppendChild(service);

            serviceType = doc.CreateElement("serviceType");
            service.AppendChild(serviceType);
            serviceType.InnerText = "urn:schemas-upnp-org:service:RenderingControl:1";

            serviceId = doc.CreateElement("serviceId");
            service.AppendChild(serviceId);
            //serviceId.InnerText = "urn:upnp-org:serviceId:AVTransport.0001";
            serviceId.InnerText = "urn:upnp-org:serviceId:RenderingControl";

            SCPDURL = doc.CreateElement("SCPDURL");
            service.AppendChild(SCPDURL);
            //SCPDURL.InnerText = "urn-schemas-upnp-org-service-AVTransport.0001_scpd.xml";
            // SCPDURL.InnerText = "serviceDescripton.xml";
            SCPDURL.InnerText = "RenderingControl/serviceDescription/";

            controlURL = doc.CreateElement("controlURL");
            service.AppendChild(controlURL);
            //controlURL.InnerText = "urn:upnp-org:serviceId:AVTransport.0001_control";
            controlURL.InnerText = "";

            eventSubUrl = doc.CreateElement("eventSubURL");
            service.AppendChild(eventSubUrl);
            eventSubUrl.InnerText = " ";
            */
            #endregion

            //for debug
            doc.Save("DeviceDescDebug.xml");
            
            SaveFile(doc.OuterXml, "");
        }

        public void SaveFile(string xml, string servicePath)
        {
            if (Directory.Exists(Path.GetDirectoryName(descriptionsPath + servicePath)) == false)
            {
                Directory.CreateDirectory(descriptionsPath + servicePath);
            }
            using (var fs = new FileStream(descriptionsPath + servicePath + filename, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(xml);
                sw.Close();
            }
        }

        public void GenServiceDescriptions()
        {
            genServiceXmlAVTransport();
            genServiceXmlRenderingControl();
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
                    //stateVariable.SetAttribute("sendEvents", "yes");

                    XmlElement name_stateVariable = doc.CreateElement("name");
                    stateVariable.AppendChild(name_stateVariable);
                    name_stateVariable.InnerText = arg.relatedStateVariable;

                    XmlElement sendEventAttribute = doc.CreateElement("sendEventAttribute");
                    stateVariable.AppendChild(sendEventAttribute);
                    sendEventAttribute.InnerText = arg.sendEventAttribute;

                    XmlElement dataType = doc.CreateElement("dataType");
                    stateVariable.AppendChild(dataType);
                    dataType.InnerText = arg.dataType;
            }

            #endregion

            //only for debug
            doc.Save(type + "Service.xml");

            SaveFile(doc.OuterXml, type + servicePath);
        }

        //Obsolete!!
        //generates Service description XML
        public void genServiceXmlAVTransport()
        {
            #region setup

            //string path = @"Descriptions\AVTransport\serviceDescription\desc.xml";
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

            #region Play action

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

            #region Pause action

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

            #region Stop action

            action = doc.CreateElement("action");
            actionList.AppendChild(action);

            name = doc.CreateElement("name");
            action.AppendChild(name);
            name.InnerText = "Stop";

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

            #region Next action

            action = doc.CreateElement("action");
            actionList.AppendChild(action);

            name = doc.CreateElement("name");
            action.AppendChild(name);
            name.InnerText = "Next";

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

            #region Previous action

            action = doc.CreateElement("action");
            actionList.AppendChild(action);

            name = doc.CreateElement("name");
            action.AppendChild(name);
            name.InnerText = "Previous";

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

            #region SetTRansportURI action

            action = doc.CreateElement("action");
            actionList.AppendChild(action);

            name = doc.CreateElement("name");
            action.AppendChild(name);
            name.InnerText = "SetAVTransportURI";

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

            argument = doc.CreateElement("argument");
            argumentList.AppendChild(argument);

            name_PlayArgument = doc.CreateElement("name");
            argument.AppendChild(name_PlayArgument);
            name_PlayArgument.InnerText = "CurrentURI";

            direction_play = doc.CreateElement("direction");
            argument.AppendChild(direction_play);
            direction_play.InnerText = "in";

            relatedStateVariable_play = doc.CreateElement("relatedStateVariable");
            argument.AppendChild(relatedStateVariable_play);
            relatedStateVariable_play.InnerText = "AVTransportURI";

            argument = doc.CreateElement("argument");
            argumentList.AppendChild(argument);

            name_PlayArgument = doc.CreateElement("name");
            argument.AppendChild(name_PlayArgument);
            name_PlayArgument.InnerText = "CurrentURIMetaData";

            direction_play = doc.CreateElement("direction");
            argument.AppendChild(direction_play);
            direction_play.InnerText = "in";

            relatedStateVariable_play = doc.CreateElement("relatedStateVariable");
            argument.AppendChild(relatedStateVariable_play);
            relatedStateVariable_play.InnerText = "AVTransportURIMetaData";


            #endregion

            #region serviceStateTable

            #region instanceID

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

            #endregion

            #region TransportPlaySpeed

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

            #region AVTransportURI

            stateVariableSpeed = doc.CreateElement("stateVariable");
            serviceStateTable.AppendChild(stateVariableSpeed);
            //stateVariable.SetAttribute("sendEvents", "yes");

            name_stateVariableSpeed = doc.CreateElement("name");
            stateVariableSpeed.AppendChild(name_stateVariableSpeed);
            name_stateVariableSpeed.InnerText = "AVTransportURI";

            sendEventAttributeSpeed = doc.CreateElement("sendEventAttribute");
            stateVariableSpeed.AppendChild(sendEventAttributeSpeed);
            sendEventAttributeSpeed.InnerText = "no";

            dataTypeSpeed = doc.CreateElement("dataType");
            stateVariableSpeed.AppendChild(dataTypeSpeed);
            dataTypeSpeed.InnerText = "string";

            #endregion

            #region AVTransportURIMetaData

            stateVariableSpeed = doc.CreateElement("stateVariable");
            serviceStateTable.AppendChild(stateVariableSpeed);
            //stateVariable.SetAttribute("sendEvents", "yes");

            name_stateVariableSpeed = doc.CreateElement("name");
            stateVariableSpeed.AppendChild(name_stateVariableSpeed);
            name_stateVariableSpeed.InnerText = "AVTransportURIMetaData";

            sendEventAttributeSpeed = doc.CreateElement("sendEventAttribute");
            stateVariableSpeed.AppendChild(sendEventAttributeSpeed);
            sendEventAttributeSpeed.InnerText = "no";

            dataTypeSpeed = doc.CreateElement("dataType");
            stateVariableSpeed.AppendChild(dataTypeSpeed);
            dataTypeSpeed.InnerText = "string";

            #endregion

            #endregion

            //for debug
            doc.Save("ServiceXMLAVTransport.xml");

            SaveFile(doc.OuterXml, AVTransportServicePath);
        }
        //Obsolete!!
        public void genServiceXmlRenderingControl()
        {
            #region setup

            string path = @"Descriptions\RenderingControl\serviceDescription\desc.xml";
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

            #region Actions

            #region SetMute

            XmlElement action = doc.CreateElement("action");
            actionList.AppendChild(action);

            XmlElement name = doc.CreateElement("name");
            action.AppendChild(name);
            name.InnerText = "SetMute";

            XmlElement argumentList = doc.CreateElement("argumentList");
            action.AppendChild(argumentList);

            #region InstanceID
            XmlElement argument = doc.CreateElement("argument");
            argumentList.AppendChild(argument);

            name = doc.CreateElement("name");
            argument.AppendChild(name);
            name.InnerText = "InstanceID";

            XmlElement direction = doc.CreateElement("direction");
            argument.AppendChild(direction);
            direction.InnerText = "in";

            XmlElement relatedStateVariable = doc.CreateElement("relatedStateVariable");
            argument.AppendChild(relatedStateVariable);
            relatedStateVariable.InnerText = "A_ARG_TYPE_InstanceID";
            #endregion

            #region Channel

            argument = doc.CreateElement("argument");
            argumentList.AppendChild(argument);

            name = doc.CreateElement("name");
            argument.AppendChild(name);
            name.InnerText = "Channel";

            direction = doc.CreateElement("direction");
            argument.AppendChild(direction);
            direction.InnerText = "in";

            relatedStateVariable = doc.CreateElement("relatedStateVariable");
            argument.AppendChild(relatedStateVariable);
            relatedStateVariable.InnerText = "A_ARG_TYPE_Channel";
            #endregion

            #region DesiredMute

            argument = doc.CreateElement("argument");
            argumentList.AppendChild(argument);

            name = doc.CreateElement("name");
            argument.AppendChild(name);
            name.InnerText = "DesiredMute";

            direction = doc.CreateElement("direction");
            argument.AppendChild(direction);
            direction.InnerText = "in";

            relatedStateVariable = doc.CreateElement("relatedStateVariable");
            argument.AppendChild(relatedStateVariable);
            relatedStateVariable.InnerText = "Mute";

            #endregion

            #endregion

            #endregion

            #region serviceStateTable

            #region instanceID

            XmlElement serviceStateTable = doc.CreateElement("serviceStateTable");
            scpd.AppendChild(serviceStateTable);

            XmlElement stateVariable = doc.CreateElement("stateVariable");
            serviceStateTable.AppendChild(stateVariable);
            stateVariable.SetAttribute("sendEvents", "no");

            name = doc.CreateElement("name");
            stateVariable.AppendChild(name);
            name.InnerText = "A_ARG_TYPE_InstanceID";

            XmlElement dataType = doc.CreateElement("dataType");
            stateVariable.AppendChild(dataType);
            dataType.InnerText = "ui4";
            
            #endregion

            #region Channel

            stateVariable = doc.CreateElement("stateVariable");
            serviceStateTable.AppendChild(stateVariable);
            stateVariable.SetAttribute("sendEvents", "no");

            name = doc.CreateElement("name");
            stateVariable.AppendChild(name);
            name.InnerText = "A_ARG_TYPE_Channel";

            dataType = doc.CreateElement("dataType");
            stateVariable.AppendChild(dataType);
            dataType.InnerText = "string";

            #endregion

            #region Mute

            stateVariable = doc.CreateElement("stateVariable");
            serviceStateTable.AppendChild(stateVariable);
            stateVariable.SetAttribute("sendEvents", "no");

            name = doc.CreateElement("name");
            stateVariable.AppendChild(name);
            name.InnerText = "Mute";

            dataType = doc.CreateElement("dataType");
            stateVariable.AppendChild(dataType);
            dataType.InnerText = "boolean";

            #endregion

            #endregion

            //for debug
            doc.Save("ServiceXMLRenderingControl.xml");

            if (Directory.Exists(Path.GetDirectoryName(descriptionsPath + RenderingControlServicePath)) == false)
            {
                Directory.CreateDirectory(descriptionsPath + RenderingControlServicePath);
            }
            if (File.Exists(descriptionsPath + RenderingControlServicePath + filename))
                File.Delete(descriptionsPath + RenderingControlServicePath + filename);
            using (var fs = new FileStream(descriptionsPath + RenderingControlServicePath + filename, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(doc.OuterXml);
                sw.Close();
            }
        }
    }
}
