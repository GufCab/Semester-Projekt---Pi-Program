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
	/// Can receive multicast messages
	/// </summary>
    public class MulticastReceiver
    {
        //multicast variables:
        private static readonly IPAddress multicastIp = IPAddress.Parse("239.255.255.250");
        private static readonly int multicastPort = 1900;

        private static UdpClient recClient;
        private static IPEndPoint recIPep;

		/// <summary>
		/// Initializes a new instance of the <see cref="UPnP_Device.UDP.MulticastReceiver"/> class.
		/// The contructor runs the private Setup method
		/// </summary>
        public MulticastReceiver()
        {
            SetupMulticastReceiver();
        }

		/// <summary>
		/// Setups the multicast receiver.
		/// </summary>
        private static void SetupMulticastReceiver()
        {
            recClient = new UdpClient();

            recIPep = new IPEndPoint(IPAddress.Any, multicastPort);
            recClient.Client.Bind(recIPep);

            recClient.JoinMulticastGroup(multicastIp);
        }

        /// <summary>
        /// Receives multicast message. Blocks if no connection is pending
        /// </summary>
        /// <returns>
        /// Received message as string
        /// </returns>
        /// <param name='ipep'>
        /// IpEndPoint of sending client.
        /// </param>
        public string ReceiveMulticast(ref IPEndPoint ipep)
        {
            Byte[] data = recClient.Receive(ref ipep);
            
            string strData = Encoding.UTF8.GetString(data);

            return strData;
        }


    }
}
