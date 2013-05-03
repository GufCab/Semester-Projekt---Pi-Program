using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPnP_Device.UDP
{
    public class UDPServer
    {
        private string _UUID;
        private int _cacheexpire;
        private string _localip;
        private int _tcpport;
        
        //private static readonly IPAddress multicastIp = IPAddress.Parse("239.255.255.250");
        //private static readonly int multicastPort = 1900;

        public MulticastSender sender;
        public MulticastReceiver receiver;

        private Thread NotifyThread;
        private Thread ReceiveThread;

        private List<Thread> threadPool = new List<Thread>();

        public UDPServer(string uuid, int cacheexpire, string localip, int tcpport)
        {
            _UUID = uuid;
            _cacheexpire = cacheexpire;
            _localip = localip;
            _tcpport = tcpport;

            sender = new MulticastSender(_UUID, _cacheexpire, _localip, _tcpport);
            receiver = new MulticastReceiver(_UUID, _cacheexpire, _localip, _tcpport);

            NotifyThread = new Thread(sender.NotifySender);
            ReceiveThread = new Thread(Run);
        }

        public void Start()
        {
            NotifyThread.Start();
            ReceiveThread.Start();
        }

        public void Run()
        {
            while (true)
            {
                string msg = receiver.ReceiveMulticast();
                ThreadPool.QueueUserWorkItem(new WaitCallback(Handler), msg);
                
                /*
                threadPool.Add(new Thread(() => Handler(msg)));     //Explained here: http://goo.gl/6uAgD
                threadPool[(threadPool.Count-1)].Start();
                */
            }
        }

        public void Handler(object obj)
        {
            string msg = (string) obj;
            //Splitting by string explained here: http://goo.gl/PSdtL
            String[] splitter = new string[] { "\r\n" };
            String[] msgArray = msg.Split(splitter, StringSplitOptions.None);   //Using overload method of split to take string array

            if (msgArray[0] == "M-SEARCH * HTTP/1.1")
            {
                Console.WriteLine("This is right");
            }
            else
            {
                Console.WriteLine("Unknown input");
            }
        }


    }
}
