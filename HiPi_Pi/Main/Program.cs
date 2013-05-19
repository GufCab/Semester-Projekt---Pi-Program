using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UPnPConfigFactory;
using UPnP_Device;
using PlaybackCtrl;
using XMLHandler;

namespace Main
{
    class Program
    {
        static void Main()
        {
            IUPnPConfigFactory sinkfact = new SinkFactory();

            UPnP u = new UPnP(sinkfact.CreatePackage());

            PlaybackControl PlayCtrl = new PlaybackControl(u);

            /*
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }

            Track t = new Track();
            t.Path = "Music/Van Halen/";
            t.DeviceIP = localIP;
            t.FileName = "Jump.mp3";
            t.Protocol = "rtsp://";
            t.Title = "Jump";
            t.Duration = 240;
            t.Artist = "Van Halen";
            t.Album = "1984";
            t.Genre = "Pop";

            List<ITrack> trackList = new List<ITrack>();
            trackList.Add(t);
            trackList.Add(t);
            trackList.Add(t);
            trackList.Add(t);

            XMLWriterPi writer = new XMLWriterPi();

            writer.ConvertITrackToXML(trackList);
            */



            Console.Read();
        }
        

    }
}
