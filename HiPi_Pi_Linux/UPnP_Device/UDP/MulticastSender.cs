using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UPnP_Device.UPnPConfig;

namespace UPnP_Device.UDP
{
	/// <summary>
	/// Multicast sender. Enables sending of multicast messages.
	/// </summary>
    public class MulticastSender
    {
        private static readonly IPAddress multicastIp = IPAddress.Parse("239.255.255.250");
        private static readonly int multicastPort = 1900;

        private static UdpClient MulticastClient;
        private static IPEndPoint remoteep;
        private static Byte[] sendBuffer;
        private IUPnPConfig _upnPConfig;

        private string _UUID;
        private int _cacheexpire;
        private string _localip;
        private int _tcpport;

        private List<string> _notify { get; private set; }

        public MulticastSender(IIpConfig ipconf, IUPnPConfig upnpconf)
        {
            _UUID = ipconf.GUID;
            _cacheexpire = upnpconf.cacheExpire;
            _localip = ipconf.IP;
            _tcpport = ipconf.TCPPort;
            _upnPConfig = upnpconf;

            SetupMulticastSender();     //Setup

            _notify = HTTPNotifygenerator(upnpconf.NT);
        }
       
        /// <summary>
        /// Sets up the multicast sender.
        /// </summary>
        private static void SetupMulticastSender()
        {
            MulticastClient = new UdpClient();
            MulticastClient.JoinMulticastGroup(multicastIp);
            remoteep = new IPEndPoint(multicastIp, multicastPort);
        }

		/// <summary>
		/// Send a unicast message
		/// </summary>
		/// <param name='s'>
		/// Message to send as a string
		/// </param>
		/// <param name='ipend'>
		/// IpEndPoint of receiver.
		/// </param>
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

		/// <summary>
		/// Sends the _notify messages.
		/// </summary>
        public void NotifySender()
        {
            while (true)
            {
                for (int l = 0; l < 4; l++)
                {
                    foreach (string s in _notify)
                    {
                        SendMulticast(s);
                    }
                    Thread.Sleep(66);
                }
                Thread.Sleep((_cacheexpire*1000));
            }
        }

        /// <summary>
        /// Sends the OK message by Unicast
        /// </summary>
        /// <param name='ipend'>
        /// IpEndPoint of receiving client
        /// </param>
        public void OKSender(IPEndPoint ipend)
        {
            string f = HTTPOKgenerator();

            Thread.Sleep(400);
            
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                byte[] sendbuf = Encoding.UTF8.GetBytes(f);
                sock.SendTo(sendbuf, ipend);
            }
        }

		/// <summary>
		/// Sends the multicast message
		/// </summary>
		/// <param name='s'>
		/// message to send
		/// </param>
        private void SendMulticast(string s)
        {
            sendBuffer = Encoding.UTF8.GetBytes(s);
            MulticastClient.Send(sendBuffer, sendBuffer.Length, remoteep);
        }

		/// <summary>
		/// Generates the HTTP messages for _notify
		/// </summary>
		/// <returns>
		/// returns a list of HTTP headers
		/// </returns>
		/// <param name='NTs'>
		/// A list of the different NTs used in the messages
		/// </param>
        private List<string> HTTPNotifygenerator(List<string> NTs)
        {
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

		/// <summary>
		/// Generates the HTTP messages for OK
		/// </summary>
		/// <returns>
		/// Returns a OK message as a string
		/// </returns>
        private string HTTPOKgenerator()
        {
            string s = "HTTP/1.1 200 OK\r\n" +
                       //"ST: " + IPHandler.GetInstance().DeviceType + "\r\n" +
                       "ST: upnp:rootdevice\r\n" + 
                       "CACHE-CONTROL: max-age=" + _cacheexpire + " \r\n" +
                       "EXT: \r\n" +
                       "USN: uuid:" + _UUID + "::" + _upnPConfig.DeviceType + "\r\n" +
                       "SERVER: Windows NT/5.0, UPnP/1.0\r\n" +
                       "LOCATION: http://" + _localip + ":" + _tcpport + "\r\n" +
                       "Content-Length: 0\r\n" + 
                       "\r\n";
            return s;
        }

    }
}
