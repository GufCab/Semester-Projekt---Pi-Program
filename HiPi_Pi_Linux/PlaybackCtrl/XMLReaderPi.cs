﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Containers;

/// <summary>
/// This namespace contains features for reading incoming XML messages
/// </summary>
namespace XMLReader
{
    /// <summary>
    /// Class that reads the xml that is received by UPnP
    /// </summary>
    public class XMLReader
    {
        /// <summary>
        /// Converts an xml string to a list of containers
        /// </summary>
        /// <param name="xml">The xml string that should be converted</param>
        /// <returns>List of containers</returns>
        public List<Container> containerReader(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var tmpList = new List<Container>();
            

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("container");

            foreach (XmlElement elm in nodeList)
            {
                var container = new Container();
                XmlNodeList titleList = elm.GetElementsByTagName("dc:title");
                container.title = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("upnp:class");
                container.upnpClass = titleList[0].InnerText;

                container.id = elm.GetAttribute("id");
                container.parentID = elm.GetAttribute("parentID");
                
                tmpList.Add(container);
                
            }
            return tmpList;
        }

        /// <summary>
        /// Converts an xml string to a list of containers
        /// </summary>
        /// <param name="xml">The xml string that should be converted</param>
        /// <returns>List of ITracks</returns>
        public List<ITrack> itemReader(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var tracks = new List<ITrack>();

            XmlNodeList nodeList = doc.GetElementsByTagName("item");

            foreach (XmlElement elm in nodeList)
            {
                ITrack track = new Track();
                Console.WriteLine("Inside Inside foreach itemReader");
                XmlNodeList titleList = elm.GetElementsByTagName("upnp:album");
                track.Album = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("dc:title");
                track.Title = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("upnp:artist");
                track.Artist = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("upnp:genre");
                track.Genre = titleList[0].InnerText;

                titleList = elm.GetElementsByTagName("res");

                string[] s = titleList[0].InnerText.Split('/');

                track.Protocol = s[0] + "//";
                track.DeviceIP = s[2];

                track.FileName = s.Last();

                string tmpString = "/";

				
                for (int i = 3; i < s.Count() - 1; i++)
                {
                    tmpString = tmpString + s[i] + "/";
					
                }

                track.Path = tmpString;
				
                track.Duration = titleList[0].Attributes["duration"].Value;
                tracks.Add(track);
            }
            return tracks;
        }
    }
}


