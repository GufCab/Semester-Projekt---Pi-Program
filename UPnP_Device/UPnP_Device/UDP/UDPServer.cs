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
        
        //Exlicit contructor. Takes arguments used for UDP communication
        public UDPServer(string uuid, int cacheexpire, string localip, int tcpport)
        {
            //Set private attributes:
            _UUID = uuid;
            _cacheexpire = cacheexpire;
            _localip = localip;
            _tcpport = tcpport;

            sender = new MulticastSender(_UUID, _cacheexpire, _localip, _tcpport);          //Creates sender
            receiver = new MulticastReceiver(_UUID, _cacheexpire, _localip, _tcpport);      //Creates receiver

            NotifyThread = new Thread(sender.NotifySender);     //Thread for notifier. Runs every _cacheexpire seconds
            ReceiveThread = new Thread(Run);                    //Run thread. The default UDP Thread
        }

        //Starts the two thread
        public void Start()
        {
            NotifyThread.Start();
            ReceiveThread.Start();
        }

        //Run Thread. Receives incoming messages and parses them to handler
        public void Run()
        {
            while (true)
            {
                IPEndPoint ipep = default(IPEndPoint);              //initiates the IPEndPont as default
                string msg = receiver.ReceiveMulticast(ref ipep);   //Blocking until new connection. A ref to ipep is parsed, and is set to the endpoint of the sender
                
                if(UDP_Debug.DEBUG) {Console.WriteLine("ipep: " + ipep.ToString());}        //Used for debuging. Set in UDP_Debug class

                object[] objPackage = new object[2];
                objPackage[0] = msg;
                objPackage[1] = ipep;
                ThreadPool.QueueUserWorkItem(new WaitCallback(Handler), objPackage);
            }
        }

        //Handler. Initiated in new thread @ incoming message
        public void Handler(object obj)
        {
            //casting parsed obj:
            object[] objArray = (object[])obj;              //Casting object to array
            string msg = (string) objArray[0];              //Cast of Object to string. This is the received message
            IPEndPoint ipend = (IPEndPoint) objArray[1];    //Cast of Object to IPEndPoint. This is the endpoint of the sender

            //Splits received message for readability
            String[] splitString = new string[] {"\r\n"};       
            String[] msgArray = msg.Split(splitString, StringSplitOptions.None);    //Making use of overloaded method string.split. Splitting at string, not char

            //Todo: Alot of refactoring needed here:
            //If the first line is from an M-SEARCH:
            if (msgArray[0] == "M-SEARCH * HTTP/1.1")
            {
                if(UDP_Debug.DEBUG) {Console.WriteLine("M-SEARCH received!");}

                //Runs through strings in array:
                foreach (string s in msgArray)
                {
                    string[] f = s.Split(':');      //Splits string again

                    STCheck(f, ipend);      //Checks for "ST"-tag
                }
            }
            else
            {
                if(UDP_Debug.DEBUG) {Console.WriteLine("Unknown input");}
            }               
        }

        private void STCheck(string[] f, IPEndPoint ipend)
        {
            if (f[0] == "ST")
            {
                string k = "";
                for (int i = 2; i < f.Count(); i++)
                {
                    k = k + f[i];
                }
                checkDeviceType(k, ipend);
            }
        }

        public void checkDeviceType(string s, IPEndPoint ipend)
        {
            if ((s.Contains(IPHandler.GetInstance().DeviceType)) | (s.Contains("rootdevice")))
            {
                sender.OKSender(ipend);
            }
        }
    }
}
