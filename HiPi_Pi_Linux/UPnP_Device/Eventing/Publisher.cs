using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace UPnP_Device
{
	public class Publisher
	{
		private List<Subscriber> _Subscribtions;

		public event Container.PropertyChangedDel;

		public Publisher ()
		{
			PropertyChangedDel += PropertChangedFunc;
		}

		public void NewSubscriber (ISubscriber sub)
		{
			_Subscribtions.Add (sub);
		}

		public void NewSubscriber (string uuid, string cburl, INetworkUtillity util)
		{
			_Subscribtions.Add (new Subscriber(uuid, cburl, util));
		}

		public void PropertyChangedFunc (object e, string PropertyName)
		{
			string msg = "";

			foreach (Subscriber sub in _Subscribtions)
			{
				//Do stuff
				//Send something


				sub.Util.Send(msg);
			}
		}
	
		private void SendEventMsg (ISubscriber sub)
		{
			foreach(string url in sub.CallbackURLs)
			{
				IPEndPoint ipep = new IPEndPoint
				TcpClient cli = new TcpClient(
			}



		}

		public string GenerateResponse(string ChangedProp, ISubscriber sub)
		{
			//string body = 

			string head = 
					"NOTIFY " + DelivPath + " HTTP/1.1\r\n" + 
					"HOST: " + OwnIP + "\r\n" + 
					"CONTENT-TYPE: text/xml\r\n" + 
					"CONTENT-LENGTH: " + body.Length + "\r\n" + 
					"NT: upnp:event\r\n" + 
					"NTS: upnp:propchange\r\n" + 
					"SID: uuid:" + sub.UUID + "\r\n" +
					"SEQ: " sub.EventNo + "\r\n";
		

			return head + "\r\n" + body;
		}
}

