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
		private List<Subscriber> _Subscribtions;

		public event PropertyChangedDel PropEvent;

		private IIpConfig ipconf;


		public Publisher (IIpConfig ip)
		{
			PropEvent += PropertyChangedFunc;
			ipconf = ip;
		}

		public void NewSubscriber (string uuid, string cburl)
		{

			_Subscribtions.Add (new Subscriber (uuid, cburl));
		}



		public void PropertyChangedFunc (object e, string PropertyName)
		{
			string val = (string)e;

			foreach (Subscriber sub in _Subscribtions)
			{
				string msg = GenerateResponse(PropertyName, val, sub);
				Thread t = new Thread(new ParameterizedThreadStart(SendEventMsg));
				object[] g = new object[] {sub, msg};

				t.Start (g);
			}
		}
	
		private void SendEventMsg (object e)
		{
			Object[] g = (object[]) e;
			TcpClient p = new TcpClient((IPEndPoint)g[0]);
			INetworkUtillity util = new NetworkUtillity(p);
			util.Send ((string)g[1]);
		}

		public string GenerateResponse(string ChangedProp, string value, ISubscriber sub)
		{
			List<UPnPArg> list = new List<UPnPArg>();
			list.Add (new UPnPArg(ChangedProp, value));
			string body = EventResponse(list);

			string head = EventHead (sub, ipconf, body.Length);
					
			return head + "\r\n" + body;
		}

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

		public string EventResponse (List<UPnPArg> args)
		{
			XmlDocument doc = new XmlDocument ();
			XmlDeclaration dec = doc.CreateXmlDeclaration ("1.0", null, null);
			doc.AppendChild (dec);

			XmlElement prop = doc.CreateElement ("e", "propertyset", "schemas-upnp-org:event-1-0");
			doc.AppendChild (prop);

			XmlElement property = doc.CreateElement ("property", "schemas-upnp-org:event-1-0");
			property.Prefix = "e";
			prop.AppendChild (property);

			foreach (UPnPArg s in args)
			{
				XmlElement variable = doc.CreateElement (s.ArgName);
				property.AppendChild (variable);
				variable.InnerText = s.ArgVal;
			}
            //Saved for debugging:
            doc.Save(@"InvokeEvent.xml");

            return doc.OuterXml;
        }
	}
}