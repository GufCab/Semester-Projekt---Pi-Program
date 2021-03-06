﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace XMLHandler
{
    public class XMLReader
    {
        public List<Result> containerReader(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load("container.xml");
            //xmlDocument.LoadXml(xml);

            var tmpList = new List<Result>();
            

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("container");

            foreach (XmlElement elm in nodeList)
            {
                var result = new Result();
                XmlNodeList titleList = elm.GetElementsByTagName("dc:title");
                
                result.id = elm.GetAttribute("id");
                result.parentID = elm.GetAttribute("parentID");
                result.ContainerRes = "Container";
                result.type = titleList[0].InnerText;

                tmpList.Add(result);
                
            }
            return tmpList;
        }

        public List<Result> itemReader(string xml)
        {
            var xmlDocument = new XmlDocument();
            //xmlDocument.Load("SomeXML.xml");
            xmlDocument.LoadXml(xml);

            var tmpList = new List<Result>();

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("item");

            foreach (XmlElement elm in nodeList)
            {
                var result = new Result();
                XmlNodeList titleList = elm.GetElementsByTagName("title");

                result.id = elm.GetAttribute("id");
                result.parentID = elm.GetAttribute("parentID");
                result.ContainerRes = "item";
                result.type = titleList[0].InnerText;

                tmpList.Add(result);
            }
            return tmpList;
        }
    }

    public class Result
    {
        public string ContainerRes { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string parentID { get; set; }
    }
}


