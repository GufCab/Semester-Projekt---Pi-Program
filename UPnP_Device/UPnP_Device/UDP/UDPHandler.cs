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
    public class UDPHandler
    {
        public MulticastSender sender;
        public MulticastReceiver receiver;

        private Thread NotifyThread;
        private Thread ReceiveThread;
        
        //Explicit contructor that takes the UPnPConfig classes:
        public UDPHandler(IIpConfig ipconf, IUPnPConfig upnpconf)
        {
            sender = new MulticastSender(ipconf, upnpconf);          //Creates sender
            receiver = new MulticastReceiver();
            //receiver = new MulticastReceiver(ipconf, upnpconf);     //Creates receiver

            NotifyThread = new Thread(sender.NotifySender);     //Thread for notifier. Runs every _cacheexpire seconds
            ReceiveThread = new Thread(Run);                    //Run thread. The default UDP Thread
        }

        /* //Todo: Should probably be removed:
        //Exlicit contructor. Takes arguments used for UDP communication
        public UDPHandler(string uuid, int cacheexpire, string localip, int tcpport)
        {
            sender = new MulticastSender(uuid, cacheexpire, localip, tcpport);          //Creates sender
            receiver = new MulticastReceiver(uuid, cacheexpire, localip, tcpport);      //Creates receiver

            NotifyThread = new Thread(sender.NotifySender);     //Thread for notifier. Runs every _cacheexpire seconds
            ReceiveThread = new Thread(Run);                    //Run thread. The default UDP Thread
        
         */

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
            object[] objArray = (object[])obj;              //Casting objects and object array
            string msg = (string) objArray[0];              
            IPEndPoint ipend = (IPEndPoint) objArray[1];    

            //Splits received message for readability
            String[] splitString = new string[] {"\r\n"};       
            String[] msgArray = msg.Split(splitString, StringSplitOptions.None);    //Making use of overloaded method string.split. Splitting at string, not char

            //Todo: Alot of refactoring needed here:
            //If the first line is from an M-SEARCH:
            if (msgArray[0] == "M-SEARCH * HTTP/1.1")
            {
                if(UDP_Debug.DEBUG) {Console.WriteLine("M-SEARCH received!");}

                foreach (string s in msgArray)
                {
                    string[] f = s.Split(':');  
                    STCheck(f, ipend);          //Checks for "ST"-tag
                }
            }
            else if (UDP_Debug.DEBUG) { Console.WriteLine("Unknown input"); }              
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
