using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Containers;
using PlaybackCtrl;

namespace XMLHandler
{
    public class XMLWriter
    {
        public string ConvertITrackToXML(List<ITrack> tracks)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("DIDL-lite");
            doc.AppendChild(root);

            root.SetAttribute("xmlns", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
            root.SetAttribute("xmlns:upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
            root.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            root.SetAttribute("xmlns:sec", "http://www.sec.co.kr/");

			int i = 0;
            foreach (ITrack track in tracks)
            {
				++i;
                XmlElement item = doc.CreateElement("item");
                root.AppendChild(item);
                item.SetAttribute("id", i.ToString()); //hvad skal id være? skal den tilføjes til track?
                item.SetAttribute("parentID", track.ParentID);  //hvor får den parentID fra?

                XmlElement title = doc.CreateElement("dc", "title", "http://purl.org/dc/elements/1.1/");
                item.AppendChild(title);
                title.InnerText = track.Title;

                XmlElement Class = doc.CreateElement("upnp", "class", "urn:schemas-upnp-org:metadata-1-0/upnp/");
                item.AppendChild(Class);
                Class.InnerText = "object.item.audioItem.musicTrack";

                XmlElement album = doc.CreateElement("upnp", "album", "urn:schemas-upnp-org:metadata-1-0/upnp/");
                item.AppendChild(album);
                album.InnerText = track.Album;

                XmlElement genre = doc.CreateElement("upnp", "genre", "urn:schemas-upnp-org:metadata-1-0/upnp/");
                item.AppendChild(genre);
                genre.InnerText = track.Genre;

                XmlElement artist = doc.CreateElement("upnp", "artist", "urn:schemas-upnp-org:metadata-1-0/upnp/");
                item.AppendChild(artist);
                artist.InnerText = track.Artist;

                XmlElement date = doc.CreateElement("upnp", "date", "urn:schemas-upnp-org:metadata-1-0/upnp/");
                item.AppendChild(date);
                date.InnerText = "NOT IMPLEMENTED";

                XmlElement res = doc.CreateElement("res");
                item.AppendChild(res);
                res.SetAttribute("duration", track.Duration.ToString());
                res.InnerText = track.Protocol + track.DeviceIP + "/" + track.Path + "/" + track.FileName;
            }

            doc.Save("CreatedXML.xml");
			string msg = doc.OuterXml;
			return msg;
        }
    }
}
