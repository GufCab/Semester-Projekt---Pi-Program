using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Containers;
using UPnP_Device.UPnPConfig;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;

namespace UPnP_Device
{


	public class Publisher
	{
		public List<Subscriber> _Subscribtions;

		//public event PropertyChangedDel PropEvent;

		private IIpConfig ipconf;


		public Publisher (IIpConfig ip)
		{
			PropertyChangedEvent.PropEvent += PropertyChangedFunc;
			ipconf = ip;
			_Subscribtions = new List<Subscriber>();
		}

		public void NewSubscriber (string uuid, string cburl)
		{
			Console.WriteLine("uuid: " + uuid);
			Console.WriteLine ("cburl: " + cburl);
			_Subscribtions.Add (new Subscriber (uuid, cburl));
		}



		public void PropertyChangedFunc (UPnPArg e)
		{
			Console.WriteLine (" >> Prop Changed Event! <<");
			string body = EventBody(e);

			foreach (Subscriber sub in _Subscribtions)
			{
				string head = EventHead(sub, ipconf, body.Length);
				string msg = head + "\r\n" + body;

				Thread t = new Thread(new ParameterizedThreadStart(SendEventMsg));
				object[] g = new object[] {sub, msg};

				t.Start (g);
			}
		}
	
		private void SendEventMsg (object e)
		{
			Object[] g = (object[]) e;
			Subscriber sub = (Subscriber)g[0];
			TcpClient p = new TcpClient();
			p.Connect(sub.ipep);

			INetworkUtillity util = new NetworkUtillity(p);

			util.Send ((string)g[1]);
			Console.WriteLine("Event msg send to CP");

			string answer = util.Receive();
			Console.WriteLine("Anwser from CP: " + answer);
		}

		/*
		public string GenerateResponse(string ChangedProp, string value, ISubscriber sub)
		{
			UPnPArg arg = new UPnPArg();
			arg (new UPnPArg(ChangedProp, value));
			string body = EventResponse(arg);

			string head = EventHead (sub, ipconf, body.Length);
					
			return head + "\r\n" + body;
		}
		*/

		public string EventHead (ISubscriber sub, IIpConfig ipconf, int length)
		{
			return  "NOTIFY /" + sub.DeliveryPath + " HTTP/1.1\r\n" + 
					"HOST: http://" + ipconf.IP + ":" + ipconf.TCPPort + "\r\n" + 
					"CONTENT-TYPE: text/xml\r\n" + 
					"CONTENT-LENGTH: " + length + "\r\n" + 
					"NT: upnp:event\r\n" + 
					"NTS: upnp:propchange\r\n" + 
					"SID: uuid:" + sub.UUID + "\r\n" +
					"SEQ: " + sub.EventNo + "\r\n";
		}

		public string EventBody (UPnPArg arg)
		{
			XmlDocument doc = new XmlDocument ();
			XmlDeclaration dec = doc.CreateXmlDeclaration ("1.0", null, null);
			doc.AppendChild (dec);

			XmlElement prop = doc.CreateElement ("e", "propertyset", "schemas-upnp-org:event-1-0");
			doc.AppendChild (prop);

			XmlElement property = doc.CreateElement ("property", "schemas-upnp-org:event-1-0");
			property.Prefix = "e";
			prop.AppendChild (property);

			XmlElement variable = doc.CreateElement (arg.ArgName);
			property.AppendChild (variable);
			variable.InnerText = arg.ArgVal;
		
            //Saved for debugging:
            doc.Save(@"InvokeEvent.xml");

            return doc.OuterXml;
        }
	}
}