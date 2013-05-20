using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Containers;
using UPnP_Device.UPnPConfig;

namespace UPnP_Device
{
	public class Publisher
	{
		private List<Subscriber> _Subscribtions;

		public event PropertyChangedDel PropEvent;

		private IIpConfig ipconf;


		public Publisher (IIpConfig ip)
		{
			PropEvent += PropertyChangedFunc;
			ipconf = ip;
		}

		public void NewSubscriber (Subscriber sub)
		{
			_Subscribtions.Add (sub);
		}

		public void NewSubscriber (string uuid, string cburl)
		{
			_Subscribtions.Add (new Subscriber(uuid, cburl));
		}

		public void PropertyChangedFunc (object e, string PropertyName)
		{
			string val = (string)e;

			foreach (Subscriber sub in _Subscribtions)
			{
				string msg = GenerateResponse(PropertyName, val, sub);


			}
		}
	
		private void SendEventMsg (ISubscriber sub)
		{


		}

		public string GenerateResponse(string ChangedProp, string value, ISubscriber sub)
		{
			string body = "Some XML";

			string head = 
					"NOTIFY /" + sub.DeliveryPath + " HTTP/1.1\r\n" + 
					"HOST: http://" + ipconf.IP + ":" + ipconf.TCPPort + "\r\n" + 
					"CONTENT-TYPE: text/xml\r\n" + 
					"CONTENT-LENGTH: " + body.Length + "\r\n" + 
					"NT: upnp:event\r\n" + 
					"NTS: upnp:propchange\r\n" + 
					"SID: uuid:" + sub.UUID + "\r\n" +
					"SEQ: " + sub.EventNo + "\r\n";
		
			return head + "\r\n" + body;
		}
	}
}