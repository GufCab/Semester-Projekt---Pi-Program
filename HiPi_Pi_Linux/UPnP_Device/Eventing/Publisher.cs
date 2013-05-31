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
    /// <summary>
    /// This class is used to keep track of subscribing control points. 
    /// </summary>
	public class Publisher
	{
		public List<Subscriber> _Subscribtions;

		//public event PropertyChangedDel PropEvent;

		private IIpConfig ipconf;

        /// <summary>
        /// Constructor
        /// Configured with IIpConfig package.
        /// </summary>
        /// <param name="ip">IP Configure package</param>
		public Publisher (IIpConfig ip)
		{
			PropertyChangedEvent.PropEvent += PropertyChangedFunc;
			ipconf = ip;
			_Subscribtions = new List<Subscriber>();
		}

        /// <summary>
        /// Add a new subscriber to the list of subscriber
        /// </summary>
        /// <param name="uuid">Unique ID for device</param>
        /// <param name="cburl">location URL for device</param>
		public void NewSubscriber (string uuid, string cburl)
		{
			Console.WriteLine("uuid: " + uuid);
			Console.WriteLine ("cburl: " + cburl);
			_Subscribtions.Add (new Subscriber (uuid, cburl));
		}

        /// <summary>
        /// Called whenever an evented variable changes state.
        /// Not implemented.
        /// </summary>
        /// <param name="e">Argument with value and type to be sent to control point</param>
		public void PropertyChangedFunc (UPnPArg e)
		{
			Console.WriteLine (" >> Prop Changed Event! <<");
			string body = EventBody (e);

			/*
			if (_Subscribtions.Count > 0)
			{
				foreach (Subscriber sub in _Subscribtions)
				{
					string head = EventHead (sub, ipconf, body.Length);
					string msg = head + "\r\n" + body;

					Thread t = new Thread (new ParameterizedThreadStart (SendEventMsg));
					object[] g = new object[] {sub, msg};

					t.Start (g);
				}
			}*/
		}
	    
        /// <summary>
        /// Actually send to the control point
        /// </summary>
        /// <param name="e"></param>
		private void SendEventMsg (object e)
		{
			Object[] g = (object[]) e;
			Subscriber sub = (Subscriber)g[0];
			TcpClient p = new TcpClient();
			p.Connect(sub.ipep);
			Console.WriteLine("IP Endpoint for events: " + sub.ipep.Address.ToString());
			INetworkUtillity util = new NetworkUtillity(p);

			util.Send ((string)g[1]);
			Console.WriteLine("Event msg sent to CP");

			//string answer = util.Receive();
			//Console.WriteLine("Anwser from CP: " + answer);
			//util.Close();
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
        /// <summary>
        /// Create the HTTP head for the event message.
        /// </summary>
        /// <param name="sub">Subscriber</param>
        /// <param name="ipconf">IP Configuration package</param>
        /// <param name="length">length of body</param>
        /// <returns></returns>
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

        /// <summary>
        /// Function to create event XML body.
        /// </summary>
        /// <param name="arg">Argument to convert to XML</param>
        /// <returns>string of XML body</returns>
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
            //doc.Save(@"InvokeEvent.xml");

            return doc.OuterXml;
        }
	}
}