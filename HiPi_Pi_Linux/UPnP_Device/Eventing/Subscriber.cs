using System;
using System.Collections.Generic;
using System.Net;
/// <summary>
/// Root namespace for all UPnP namespaces and base classes.
/// </summary>
namespace UPnP_Device
{
    /// <summary>
    /// Interface exposing the functionality and public members of Subscribers.
    /// </summary>
	public interface ISubscriber
	{
		string UUID { get; set; }
		NetworkUtillity Util { get; set; }
		int EventNo { get; set; }

		IPEndPoint ipep { get; set; }
		string DeliveryPath {get;set;}
	}

    /// <summary>
    /// Class used to represent a subscribing control point in the system. 
    /// </summary>
	public class Subscriber : ISubscriber
	{
		public string UUID {get; set;}
		public string DeliveryPath {get; set;}
		public NetworkUtillity Util {get; set;}
		public IPEndPoint ipep {get; set;}


		private int _EventNo;
		
        /// <summary>
        /// The number of event sent.
        /// Wraps around the max value 4294967294
        /// </summary>
        public int EventNo {
			set {
				if (value >= 4294967294)
					_EventNo = 1;
				else
					_EventNo = value;
			}
			get { return _EventNo; }
		}

        /// <summary>
        /// Constructor.
        /// Creates the constructor with the information needed for generating af response to a control point.
        /// </summary>
        /// <param name="uuid">Unique device ID</param>
        /// <param name="cburl">Device location</param>
		public Subscriber (string uuid, string cburl)
		{
			EventNo = 0;

			UUID = uuid;

			string delPath = "";

			string[] s = cburl.Split('/');
			string[] f = s[2].Split (':');
			IPAddress ip = IPAddress.Parse (f[0]);
			int port = Convert.ToInt32(f[1]);

			ipep = new IPEndPoint(ip, port);

			for(int i = 3; i < s.Length; i++)
			{
				delPath = delPath + s[i] + "/";
			}

			DeliveryPath = delPath;

			Console.WriteLine ("=================================");
			Console.WriteLine (" >> New Subscriber:");
			Console.WriteLine ("IP: " + ip.ToString ());
			Console.WriteLine("Port: " + port.ToString ());
			Console.WriteLine ("Delevery Path: " + delPath);


		}
	}
}

