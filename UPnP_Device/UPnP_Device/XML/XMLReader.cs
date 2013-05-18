using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace UPnP_Device.XML
{
    public class XMLReader
    {
        public List<Arguments> ReadArguments(string xml, string actionName)
        {
            var args = new List<Arguments>();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("u:" + actionName);

            try
            {
                foreach (XmlElement elm in nodeList[0].ChildNodes)
                {
                    args.Add(new Arguments(elm.Name, elm.InnerText));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in XML reader!");
                Console.WriteLine(e);
                throw e;
            }
            
            return args;
        }
    }

    public class Arguments
    {
        public Arguments(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string _name { set; get; }
        public string _value { set; get; }
    }
}
