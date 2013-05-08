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
                IPEndPoint ipep = default(IPEndPoint);
                string msg = receiver.ReceiveMulticast(ref ipep);
                Console.WriteLine("ipep: " + ipep.ToString());

                object[] myArray = new object[2];
                myArray[0] = msg;
                myArray[1] = ipep;
                ThreadPool.QueueUserWorkItem(new WaitCallback(Handler), myArray);

                /*
                threadPool.Add(new Thread(() => Handler(msg)));     //Explained here: http://goo.gl/6uAgD
                threadPool[(threadPool.Count-1)].Start();
                */
            }
        }

        public void Handler(object obj)
        {
            object[] u = (object[]) obj;
            string msg = (string) u[0];
            IPEndPoint ipend = (IPEndPoint) u[1];

            //Splitting by string explained here: http://goo.gl/PSdtL
            String[] splitter = new string[] {"\r\n"};
            String[] msgArray = msg.Split(splitter, StringSplitOptions.None);
            //Using overload method of split to take string array

            bool ret = false;
            if (msgArray[0] == "M-SEARCH * HTTP/1.1")
            {
                Console.WriteLine("M-SEARCH received!");
                string k = "";

                foreach (string s in msgArray)
                {
                    string[] f = s.Split(':');

                    //Hvis der er ST på den første plads, så skal den sætte de resterende sammen og sende dem til checkeren
                    if (f[0] == "ST")
                    {
                        for (int i = 2; i < f.Count(); i++)
                        {
                            k = k + f[i];
                        }
                        ret = checkDeviceType(k);
                    }
                }
            }
            else
            {
                //Console.WriteLine("Unknown input");
            }

            if(ret)
                sender.OKSender(ipend);
        }

        public bool checkDeviceType(string s)
        {
            if ((s.Contains(IPHandler.GetInstance().DeviceType)) | (s.Contains("rootdevice")))
                return true;
            return false;
        }
    }
}
