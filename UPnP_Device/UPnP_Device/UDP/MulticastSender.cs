using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPnP_Device.UDP
{
    public class MulticastSender
    {
        private static readonly IPAddress multicastIp = IPAddress.Parse("239.255.255.250");
        private static readonly int multicastPort = 1900;

        private static UdpClient MulticastClient;
        private static IPEndPoint remoteep;
        private static Byte[] sendBuffer;

        private string _UUID;
        private int _cacheexpire;
        private string _localip;
        private int _tcpport;

        public List<string> NTs;
        public List<string> notify { get; private set; }

        //Constructor
        public MulticastSender(string uuid, int cacheexpire, string localip, int tcpport)
        {
            _UUID = uuid;
            _cacheexpire = cacheexpire;
            _localip = localip;
            _tcpport = tcpport;


            SetupMulticastSender();     //Setup

            //Create NTs:
            NTs = new List<string>
                {
                    "upnp:rootdevice",
                    "urn:schemas-upnp-org:device:MediaRenderer:1",
                    "urn:schemas-upnp-org:service:AVTransport:1",
                    "urn:schemas-upnp-org:service:RenderingControl:1"
                };

            notify = HTTPNotifygenerator(NTs);
        }
        
        //Setup:
        private static void SetupMulticastSender()
        {
            MulticastClient = new UdpClient();
            MulticastClient.JoinMulticastGroup(multicastIp);
            remoteep = new IPEndPoint(multicastIp, multicastPort);
        }

        private void SendUnicast(string s, IPEndPoint ipend)
        {
            using (var Unicast = new UdpClient())
            {
                Unicast.Connect(ipend);
                byte[] buf = Encoding.UTF8.GetBytes(s);

                Unicast.Send(buf, buf.Length);
                Unicast.Close();
            }
        }

        public void NotifySender()
        {
            while (true)
            {
                for (int l = 0; l < 4; l++)
                {
                    foreach (string s in notify)
                    {
                        SendMulticast(s);
                    }
                    Thread.Sleep(66);
                }
                Thread.Sleep((_cacheexpire*1000));
            }
        }

        //Needs cleanup:
        public void OKSender(IPEndPoint ipend)
        {
            string f = HTTPOKgenerator();

            Thread.Sleep(400);
            
            //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);

            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                byte[] sendbuf = Encoding.UTF8.GetBytes(f);
                sock.SendTo(sendbuf, ipend);
            }
        }

        private void SendMulticast(string s)
        {
            sendBuffer = Encoding.UTF8.GetBytes(s);
            MulticastClient.Send(sendBuffer, sendBuffer.Length, remoteep);
        }

        private List<string> HTTPNotifygenerator(List<string> NTs)
        {
            int i = 0;
            List<string> slist = new List<string>();

            string id = "uuid:" + _UUID;

            slist.Add("NOTIFY * HTTP/1.1\r\n" +
                      "HOST: " + multicastIp.ToString() + ":" + multicastPort + "\r\n" +
                      "CACHE-CONTROL: max-age=" + _cacheexpire + "\r\n" +
                      "LOCATION: http://" + _localip + "\r\n" +
                      "SERVER: Windows NT/5.0, UPnP/1.0\r\n" +
                      "NT: " + id + "\r\n" +
                      "NTS: ssdp:alive\r\n" +
                      "USN: " + id + "\r\n" + 
                      "Content-Length: 0" + "\r\n");

            foreach (string f in NTs)
            {
                slist.Add("NOTIFY * HTTP/1.1\r\n" +
                      "HOST: " + multicastIp.ToString() + ":" + multicastPort + "\r\n" +
                      "CACHE-CONTROL: max-age=" + _cacheexpire + "\r\n" +
                      "LOCATION: http://" + _localip + ":" + _tcpport + "\r\n" +
                      "SERVER: Windows NT/5.0, UPnP/1.0\r\n" +
                      "NT: " + f + "\r\n" +
                      "NTS: ssdp:alive\r\n" +
                      "USN: " + id + "::" + f + "\r\n" +
                      "Content-Length: 0" + "\r\n" + 
                      "\r\n");
            }

            return slist;
        }


        private string HTTPOKgenerator()
        {
            string s = "HTTP/1.1 200 OK\r\n" +
                       //"ST: " + IPHandler.GetInstance().DeviceType + "\r\n" +
                       "ST: upnp:rootdevice\r\n" + 
                       "CACHE-CONTROL: max-age=" + _cacheexpire + " \r\n" +
                       "EXT: \r\n" +
                       "USN: uuid:" + _UUID + "::" + IPHandler.GetInstance().DeviceType + "\r\n" +
                       //"USN: " + _UUID + "::" + IPHandler.GetInstance().DeviceType + "\r\n" +
                       "SERVER: Windows NT/5.0, UPnP/1.0\r\n" +
                       "LOCATION: http://" + _localip + ":" + _tcpport + "\r\n" +
                       "Content-Length: 0\r\n" + 
                       "\r\n";
            return s;
        }

    }
}
