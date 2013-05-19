using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Containers;
using PlaybackCtrl;

namespace XMLReader
{
    public class XMLReader1
    {
        public List<Container> containerReader(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load("container.xml");
            //xmlDocument.LoadXml(xml);

            var tmpList = new List<Container>();
            

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("container");

            foreach (XmlElement elm in nodeList)
            {
                var container = new Container();
                XmlNodeList titleList = elm.GetElementsByTagName("dc:title");
                container.title = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("dc:title");
                container.upnpClass = titleList[0].InnerText;

                container.id = elm.GetAttribute("id");
                container.parentID = elm.GetAttribute("parentID");
                
                tmpList.Add(container);
                
            }
            return tmpList;
        }

        public List<ITrack> itemReader(string xml)
        {
            var doc = new XmlDocument();
            doc.Load("item.xml");
            //xmlDocument.LoadXml(xml);

            var tracks = new List<ITrack>();

            XmlNodeList nodeList = doc.GetElementsByTagName("item");

            foreach (XmlElement elm in nodeList)
            {
                tracks.Add(new Track());

                XmlNodeList titleList = elm.GetElementsByTagName("upnp:album");
                tracks[0].Album = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("dc:title");
                tracks[0].Title = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("upnp:artist");
                tracks[0].Artist = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("upnp:genre");
                tracks[0].Genre= titleList[0].InnerText;

                //ToDo string split path and ip
                titleList = elm.GetElementsByTagName("res");
                tracks[0].Path = titleList[0].InnerText;

                //ToDo string split path and ip
                tracks[0].DeviceIP = titleList[0].InnerText;

                tracks[0].Duration = titleList[0].Attributes["duration"].Value;
                tracks[0].Duration = titleList[0].Attributes["protocolInfo"].Value;
            }
            return tracks;
        }
    }
}


