using System;
using System.Collections.Generic;
using System.Net;

namespace UPnP_Device
{
	public interface ISubscriber
	{
		string UUID { get; set; }
		NetworkUtillity Util { get; set; }
		int EventNo { get; set; }

		IPEndPoint ipep { get; set; }
		string DeliveryPath {get;set;}
	}

	public class Subscriber : ISubscriber
	{
		public string UUID {get; set;}
		public string DeliveryPath {get; set;}
		public NetworkUtillity Util {get; set;}
		public IPEndPoint ipep {get; set;}


		private int _EventNo;
		public int EventNo {
			set {
				if (value >= 4294967294)
					_EventNo = 1;
				else
					_EventNo = value;
			}
			get { return _EventNo; }
		}



		public Subscriber (string uuid, string cburl)
		{
			EventNo = 0;

			UUID = uuid;

			string delPath = "";

			string[] s = cburl.Split('/');
			string[] f = s[1].Split (':');
			IPAddress ip = IPAddress.Parse (f[0]);
			int port = Convert.ToInt32(f[1]);

			ipep = new IPEndPoint(ip, port);

			for(int i = 2; i < s.Length; i++)
			{
				delPath = delPath + s[2];
			}


			}
		}
	}

