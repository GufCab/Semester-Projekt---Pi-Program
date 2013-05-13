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
        public List<string> ReadArguments(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("argumentName");
            List<string> stringList = new List<string>();

            foreach (XmlElement elm in nodeList)
            {
                stringList.Add(elm.InnerText);
            }

            return stringList;
        }
    }
}
