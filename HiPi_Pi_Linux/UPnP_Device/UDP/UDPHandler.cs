using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UPnP_Device.UPnPConfig;
using UPnP_Device.UDP;

namespace UPnP_Device.UDP
{
	/// <summary>
	/// UDPHandler handles incoming messages
	/// </summary>
	public partial class UDPHandler
	{
		public MulticastSender sender;
        //public MulticastReceiver receiver;

        private IUPnPConfig _upnPConfig; 

		/// <summary>
		/// Initializes a new instance of the <see cref="UPnP_Device.UDP.UDPHandler"/> class.
		/// </summary>
		/// <param name='send'>
		/// An instance of the MulticastSender
		/// </param>
		/// <param name='conf'>
		/// Configuration information about the UPnP Device
		/// </param>
		public UDPHandler(MulticastSender send, IUPnPConfig conf)
		{
			sender = send;
			_upnPConfig = conf;
		}


		/// <summary>
		/// Invoked from at thread in the ThreadPool. Handles the incoming messages to 
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
        public void Handle(object obj)
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

		/// <summary>
		/// Checks if ST is present in received Message
		/// </summary>
		/// <param name='f'>
		/// An array of strings. One of these should be "ST"
		/// </param>
		/// <param name='ipend'>
		/// IpEndPoint used if ST is present in the message.
		/// </param>
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

		/// <summary>
		/// Checks if the requested device type is known
		/// </summary>
		/// <param name='s'>
		/// String to check for devicetype
		/// </param>
		/// <param name='ipend'>
		/// IpendPint to respond to
		/// </param>
        private void checkDeviceType(string s, IPEndPoint ipend)
        {
            if ((s.Contains(_upnPConfig.DeviceType)) | (s.Contains("rootdevice")))
            {
                sender.OKSender(ipend);
            }
        }


	}
}

