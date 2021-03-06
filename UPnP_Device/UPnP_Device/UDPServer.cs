﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPnP_Device
{
    public class UDPServer
    {
        private static string _UUID;
        private static int _cacheexpire;
        private static string _localip;
        
        private static readonly IPAddress multicastIp = IPAddress.Parse("239.255.255.250");
        private static readonly int multicastPort = 1900;

        private static UdpClient sendClient;
        private static IPEndPoint remoteep;
        private static Byte[] sendBuffer;

        public List<string> notify { get; private set; }
        private Thread NotifyThread;
        private Thread ReceiveThread;

        
        public UDPServer(string uuid, int cacheexpire, string localip, int tcpport)
        {
            _UUID = uuid;
            _cacheexpire = cacheexpire;
            _localip = localip;

            //Setup multicast UDP socket
            SetupMulticastSender();

            //Create NTs:
            List<string> NTs = new List<string>();
            NTs.Add("upnp:rootdevice");
            NTs.Add("urn:schemas-upnp-org:device:MediaServer:1");
            NTs.Add("urn:schemas-upnp-org:service:ContentDirectory:1");
            NTs.Add("urn:schemas-upnp-org:service:ConnectionManager:1");

            List<string> notify = HTTPNotifygenerator(NTs);
            
            NotifyThread = new Thread(Notifier);
        }

        public void Start()
        {
            NotifyThread.Start();
        }

        public void Notifier()
        {
            for (int l = 0; l < 4; l++)
            {
                foreach (string s in notify)
                {
                    SendMulticast(s);
                    Console.WriteLine(s);
                }
                Thread.Sleep(66);
            }
            Thread.Sleep(_cacheexpire);
        }


        #region Send multicast

        private static void SetupMulticastSender()
        {
            sendClient = new UdpClient();

            sendClient.JoinMulticastGroup(multicastIp);

            remoteep = new IPEndPoint(multicastIp, multicastPort);
        }

        public static void SendMulticast(string s)
        {
            sendBuffer = Encoding.UTF8.GetBytes(s);
            sendClient.Send(sendBuffer, sendBuffer.Length, remoteep);
        }
        #endregion


        public static List<string> HTTPNotifygenerator(List<string> NTs)
        {
            int i = 0;
            List<string> slist = new List<string>();

            string id = "uuid:" + _UUID;

            slist.Add("NOTIFY * HTTP/1.1\r\n" +
                    "HOST: " + multicastIp.ToString() + ":" + multicastPort + "\r\n" +
                    "CACHE-CONTROL: max-age=" + _cacheexpire + "\r\n" +
                    "LOCATION: " + _localip + "\r\n" +
                    "SERVER: Windows NT/5.0, UPnP/1.1\r\n" +
                    "NT: " + id + "\r\n" +
                    "NTS: ssdp:alive\r\n" +
                    "USN: " + id + "\r\n" +
                    "Content-Length: 0" + "\r\n");

            foreach (string f in NTs)
            {
                slist.Add("NOTIFY * HTTP/1.1\n" +
                      "HOST: " + multicastIp.ToString() + ":" + multicastPort + "\r\n" +
                      "CACHE-CONTROL: max-age=" + _cacheexpire + "\r\n" +
                      "LOCATION: " + _localip + "\r\n" +
                      "SERVER: Windows NT/5.0, UPnP/1.1\r\n" +
                      "NT: " + f + "\r\n" +
                      "NTS: ssdp:alive\n" +
                      "USN: " + _UUID + "::" + f + "\r\n" +
                      "Content-Length: 0" + "\r\n");
            }

            return slist;
        }
    }
}
