using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace UPnP_Device
{
    class XMLReader
    {
        public List<Tuple<string, string>> ReadArguments(string xml, string actionName)
        {
            List<Tuple<string, string>> stringList = new List<Tuple<string, string>>();

            var xmlDocument = new XmlDocument();
            //xmlDocument.LoadXml(xml);
            xmlDocument.Load("XMLTilSvejstrup.txt");

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("u:" + actionName);
            
            foreach (XmlElement elm in nodeList[0].ChildNodes)
            {
                stringList.Add(new Tuple<string, string>(elm.Name, elm.InnerText));
            }
            
            return stringList;
        }
    }
}
