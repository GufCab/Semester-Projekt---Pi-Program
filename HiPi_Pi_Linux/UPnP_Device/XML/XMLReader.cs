using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Containers;

namespace UPnP_Device.XML
{
    /// <summary>
    /// Reads xml received via UPnP
    /// </summary>
    public class XMLReader
    {
        /// <summary>
        /// Reads an xml string and extracts the arguments of the specified function
        /// </summary>
        /// <param name="xml">XML string to be read</param>
        /// <param name="actionName">The action that the reader should look for</param>
        /// <returns>A list of UPnPArg object that holds the argumentname and argumentvalue</returns>
        public List<UPnPArg> ReadArguments(string xml, string actionName)
        {
            var args = new List<UPnPArg>();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("u:" + actionName);

            try
            {
                foreach (XmlElement elm in nodeList[0].ChildNodes)
                {
                    args.Add(new UPnPArg(elm.Name, elm.InnerText));
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
}
