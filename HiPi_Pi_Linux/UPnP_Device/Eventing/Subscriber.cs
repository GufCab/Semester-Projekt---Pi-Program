using System;
using System.Collections.Generic;
using System.Net;

namespace UPnP_Device
{
	public interface ISubscriber
	{
		List<string> CallbackURLs { get; set; }
		string UUID { get; set; }
		NetworkUtillity Util { get; set; }
		int eventNo;

		IPEndPoint ipep;
	}

	public class Subscriber : ISubscriber
	{
		public IPEndPoint ipep;
		public List<string> CallbackURLs;
		public string UUID;
		public NetworkUtillity Util;
		public int EventNo {
			set {
				if (value >= 4294967295)
					_EventNo = 1;
				else
					_EventNo = value;
			}
			get { return _EventNo; }
		}



		public Subscriber (string uuid, string cburl, INetworkUtillity util)
		{
			eventNo = 0;

			UUID = uuid;
			CallbackURL = cburl;
			Util = util;

			string[] s = url.Split(':');
			string ip = s[1].Replace ("//", "");
			string port = s[2];
			IPEndPoint ipep = new IPEndPoint(ip, port);
		}
	}
}

