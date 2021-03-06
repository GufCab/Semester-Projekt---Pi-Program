using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UPnP_Device.UPnPConfig;

/// <summary>
/// Namespace containing all UDP related UPnP functionality
/// </summary>
namespace UPnP_Device.UDP
{
    /// <summary>
    /// Interface hiding the implementation of the UDPServer functionality
    /// </summary>
    public interface IUDPServer
    {
        /// <summary>
        /// Starts the threads of the UDP Server
        /// </summary>
        void Start();
    }

    /// <summary>
    /// Class implementing the IUDPServer interface. 
    /// 
    /// This class is used to send, receive and handle UDP messages from and to control points.
    /// </summary>
    public class UDPServer : IUDPServer
    {
        public MulticastSender sender;
        public MulticastReceiver receiver;

        private Thread NotifyThread;
        private Thread ReceiveThread;

        private IIpConfig _ip;
        private IUPnPConfig _upnPConfig; 

		private UDPHandler hand;

        /// <summary>
        /// Initializes a new instance of the <see cref="UPnP_Device.UDP.UDPServer"/> class.
		/// </summary>
        /// <param name='ipconf'>
        /// ipconf is the configuration package containing information about the network util.
        /// </param>
        /// <param name='upnpconf'>
        /// Upnpconf contains information about the UPnP device
        /// </param>
        public UDPServer(IIpConfig ipconf, IUPnPConfig upnpconf)
        {
            sender = new MulticastSender(ipconf, upnpconf);          //Creates sender
            receiver = new MulticastReceiver();
            //receiver = new MulticastReceiver(IpConf, upnpconf);     //Creates receiver

            NotifyThread = new Thread(sender.NotifySender);     //Thread for notifier. Runs every _cacheexpire seconds
            ReceiveThread = new Thread(Run);                    //Run thread. The default UDP Thread

            _ip = ipconf;
            _upnPConfig = upnpconf;
			hand = new UDPHandler(sender, _upnPConfig);
        }

        /// <summary>
        /// Starts the threads of the UDP Server
        /// </summary>
        public void Start()
        {
            NotifyThread.Start();
            ReceiveThread.Start();
        }

        /// <summary>
        /// Run Thread. Receives incoming messages and parses them to handler
        /// </summary>
        private void Run()
        {
            while (true)
            {
                IPEndPoint ipep = default(IPEndPoint);              //initiates the IPEndPont as default
                string msg = receiver.ReceiveMulticast(ref ipep);   //Blocking until new connection. A ref to ipep is parsed, and is set to the endpoint of the sender
                if(UDP_Debug.MSG)
					Console.WriteLine ("New UDP message received");

                if(UDP_Debug.DEBUG) {Console.WriteLine("ipep: " + ipep.ToString());}        //Used for debuging. Set in UDP_Debug class

                object[] objPackage = new object[2];
                objPackage[0] = msg;
                objPackage[1] = ipep;
                //ThreadPool.QueueUserWorkItem(new WaitCallback(hand.Handle), objPackage);
				Thread t = new Thread(new ParameterizedThreadStart(hand.Handle));
				t.Start (objPackage);
            }
        }

		/*
		/// <summary>
		/// Handles incoming messages by parsing them to an instance of UDPHandler. This is a new Thread that runs on the ThreadPool.
		/// </summary>
		/// <param name='e'>
		/// An object containing information about received message
		/// </param>
		private void Handle(object e)
		{

			hand.Handle(e);
		}
		*/

        
    }
}
