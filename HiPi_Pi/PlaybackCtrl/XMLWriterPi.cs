using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using PlaybackCtrl;

namespace XMLHandler
{
    public class XMLWriterPi
    {
        public string testXMLConverter()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("DIDL-lite");
            doc.AppendChild(root);

            root.SetAttribute("xmlns", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
            root.SetAttribute("xmlns:upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
            root.SetAttribute("xmlns:dc" , "http://purl.org/dc/elements/1.1/");
            root.SetAttribute("xmlns:sec", "http://www.sec.co.kr/");

            XmlElement item = doc.CreateElement("item");
            root.AppendChild(item);
            item.SetAttribute("id", "Lionel Richie");

            XmlElement title = doc.CreateElement("dc:title");
            item.AppendChild(title);
            title.InnerText = "Penny Lover";

            XmlElement Class = doc.CreateElement("upnp:class");
            item.AppendChild(Class);
            Class.InnerText = "object.item.audioItem.musicTrack";

            XmlElement album = doc.CreateElement("upnp:album");
            item.AppendChild(album);
            album.InnerText = "Can't Slow Down";

            XmlElement genre = doc.CreateElement("upnp:genre");
            item.AppendChild(genre);
            genre.InnerText = "Rock";

            XmlElement artist = doc.CreateElement("upnp:artist");
            item.AppendChild(artist);
            artist.InnerText = "Lionel Richie";

            XmlElement date = doc.CreateElement("upnp:date");
            item.AppendChild(date);
            date.InnerText = "1983-01-01";

            XmlElement res = doc.CreateElement("res");
            item.AppendChild(res);
            res.SetAttribute("duration", "0:05:35.000");
            res.InnerText = "rtsp://192.168.1.100";

            item = doc.CreateElement("item");
            root.AppendChild(item);

            album = doc.CreateElement("upnp:album");
            item.AppendChild(album);


            //Console.WriteLine("Display the modified XML...");
            //doc.Save(Console.Out);
            doc.Save("test.xml");
            return doc.OuterXml;
        }

        public string ConvertITrackToXML(List<ITrack> tracks)
        {
            XmlDocument doc = new XmlDocument();

            //XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            //doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("DIDL-lite");
            doc.AppendChild(root);

            root.SetAttribute("xmlns", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
            root.SetAttribute("xmlns:upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
            root.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            root.SetAttribute("xmlns:sec", "http://www.sec.co.kr/");

            foreach (ITrack track in tracks)
            {
                XmlElement item = doc.CreateElement("item");
                root.AppendChild(item);
                item.SetAttribute("id", "1"); //hvad skal id være? skal den tilføjes til track?
                item.SetAttribute("parentID", "0");  //hvor får den parentID fra?

                XmlElement title = doc.CreateElement("dc:title");
                item.AppendChild(title);
                title.InnerText = track.Title;

                XmlElement Class = doc.CreateElement("upnp:class");
                item.AppendChild(Class);
                Class.InnerText = "object.item.audioItem.musicTrack";

                XmlElement album = doc.CreateElement("upnp:album");
                item.AppendChild(album);
                album.InnerText = track.Album;

                XmlElement genre = doc.CreateElement("upnp:genre");
                item.AppendChild(genre);
                genre.InnerText = track.Genre;

                XmlElement artist = doc.CreateElement("upnp:artist");
                item.AppendChild(artist);
                artist.InnerText = track.Artist;

                XmlElement date = doc.CreateElement("upnp:date");
                item.AppendChild(date);
                date.InnerText = "NOT IMPLEMENTED";

                XmlElement res = doc.CreateElement("res");
                item.AppendChild(res);
                res.SetAttribute("duration", track.Duration.ToString());
                res.InnerText = track.Protocol + track.DeviceIP;
            }

            doc.Save("CreatedXML.xml");
            
            return doc.OuterXml;
        }
        /*
        public string ConvertIElementToXML(List<Element> elements)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("DIDL-lite");
            doc.AppendChild(root);

            foreach (Element element in elements)
            {
                
            }

            return doc.OuterXml;
        }*/
    }

    class Element
    {
        private string _title { set; get; }
        private string _creator { set; get; } 
        private string _upnpClass { set; get; }
    }
}
