﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace UPnP_Device.XML
{
    class XMLReader
    {
        public List<Tuple<string, string>> ReadArguments(string xml, string actionName)
        {
            List<Tuple<string, string>> stringList = new List<Tuple<string, string>>();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("u:" + actionName);

            try
            {
                foreach (XmlElement elm in nodeList[0].ChildNodes)
                {
                    stringList.Add(new Tuple<string, string>(elm.Name, elm.InnerText));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in XML reader!");
                Console.WriteLine(e);
                throw e;
            }
            
            
            return stringList;
        }
    }
}