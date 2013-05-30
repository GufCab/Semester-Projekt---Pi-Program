using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;
using System.Xml.Linq;
using UPnP_Device.UPnPConfig;
using UPnP_Device.XML;
using Containers;

namespace UPnP_Device.TCP
{
    /// <summary>
    /// Reads description files and outputs as strings
    /// </summary>
    public class DescriptionReader
    {
        private string _path ;
        
        public DescriptionReader(string path)
        {
            _path = path + "/desc.xml";
        }
        
        public string Read()
        {
            string s = "-1";
           
            if (File.Exists(_path))
            {
                using (var sr = new StreamReader(_path, Encoding.UTF8))
                {
                    s = "";
                    s = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return s;
        }
    }
    
    /// <summary>
    /// Response strategy, when the HTTP request was GET
    /// </summary>
    public class GETResponder : IRespondStrategy
    {
        //private XMLWriter1 writer;
        private DescriptionReader XMLreader;

        /// <summary>
        /// Reads the description at path
        /// </summary>
        /// <param name="path">Path of requested description</param>
        public GETResponder(string path)
        {
            XMLreader = new DescriptionReader(path);
        }

        /// <summary>
        /// Handle message and respond to control point.
        /// Reads description at specified path, using XMLreader,
        /// Creates HTTP message and sends back to control point.
        /// </summary>
        /// <param name="utillity"></param>
        public void Respond (INetworkUtillity utillity)
		{
			string body = XMLreader.Read ();
			string head = "HTTP/1.1 200 OK\r\n" +
				"CONTENT-TYPE: text/xml;charset=utf-8\r\n" +
				"CONTENT-LENGTH: " + body.Length + "\r\n";
            
			string message = head + "\r\n" + body;

			if (TCPDebug.DEBUG) {
				Console.WriteLine ("TCP Message: \r\n" + message);
			}

            try
            {
                utillity.Send(message);
            }
            catch (Exception)
            {
                Console.WriteLine("Exception in Reponse");
                throw;
            }
        }
        
        
    }

    /// <summary>
    /// Response strategy used when the HTTP request was POST
    /// </summary>
    public class POSTResponder: IRespondStrategy
    {
        private string message = "";
        private string action = "";
        private IOrder orderClass;

        /// <summary>
        /// Constructor.
        /// 
        /// Takes received HTTP request as arg.
        /// </summary>
        /// <param name="ctrl">Entire HTTP request, including XML</param>
        public POSTResponder(string ctrl)
        {
            message = ctrl;
        }

        /// <summary>
        /// Handle message and respond to control point.
        /// 
        /// Determines arguments and UPnP action type.
        /// Creates Order to handle the action.
        /// </summary>
        /// <param name="util"></param>
        public void Respond (INetworkUtillity util)
		{
			action = DetermineOrder (message);
			var args = DetermineArgs (message, action);

			if (TCPDebug.DEBUG) {
				Console.WriteLine ("\nPost action was: " + action);
				Console.WriteLine ("With arguments: ");
			}

			if (TCPDebug.DEBUG) {
				foreach (var arg in args) {
					Console.Write (arg.ArgName + ": " + arg.ArgVal);
				}
			}
            
            orderClass = new Order(action, util);
            orderClass.execOrder(args);
        }

        /// <summary>
        /// Used to determine Action Type from sent message.
        /// </summary>
        /// <param name="ctl">Entire HTTP message send from Control Point</param>
        /// <returns>Action Type</returns>
        public string DetermineOrder(string ctl)
        {
            string[] splitter = new string[] {"\r\n"};
            string[] splitOrder = ctl.Split(splitter, StringSplitOptions.None);
            string soapLine = "";

            foreach (var s in splitOrder)
            {
                if (s.Contains("SOAPACTION"))
                {
                    soapLine = s;
                    break;
                }
            }

            string[] urn = soapLine.Split(':');
            string versionAndAction = urn.Last();
            string[] action = versionAndAction.Split('#')[1].Split('"');
            

            return action[0];
        }

        /// <summary>
        /// Used for creating a list of UPnP arguments 
        /// from XML, sent with HTTP request send by Control Point.
        /// </summary>
        /// <param name="wholeMessage">Entire HTTP message + XML sent from Control Point</param>
        /// <param name="actionName">Type of action</param>
        /// <returns></returns>
        public List<UPnPArg> DetermineArgs(string wholeMessage, string actionName)
        {
            string[] splitter = new string[] { "\r\n\r\n" };
            string[] HeadAndBody = wholeMessage.Split(splitter, StringSplitOptions.None);
            
            XMLReader reader = new XMLReader();
            var args = reader.ReadArguments(HeadAndBody[1], actionName);

            return args;
        }
    }

    /// <summary>
    /// Response strategy used when the HTTP request was Subscribe.
    /// Used to handle subscriptions and renewals
    /// </summary>
	public class SubscribeResponder : IRespondStrategy
	{
		private Publisher _pub;
		private string _rec;

        /// <summary>
        /// Constructor.
        /// Takes the Publisher and the received message.
        /// </summary>
        /// <param name="pub">Publisher keeping track of all subscriptions to device</param>
        /// <param name="received">Received message</param>
		public SubscribeResponder (Publisher pub, string received)
		{
			_pub = pub;
			_rec = received;
		}

		/// <summary>
		/// Get data from message and send them to publisher. 
		/// Sends correct event data back to Control point
		/// based on message type. 
		/// </summary>
		/// <param name="util">NetworkUtility used to handle communication with Control Point</param>
		public void Respond (INetworkUtillity util)
		{
			string SID = GetSid (_rec);
			string createdHead;

			if (SID == null) 
			{
			
				string subscriberUrl = GetURL (_rec);
				string uuid = Guid.NewGuid ().ToString ();

				createdHead = GetHeadNewSubscriberResponse (_rec, uuid);
				_pub.NewSubscriber (uuid, subscriberUrl);
			} 
			else 
			{
				createdHead = GetHeadRenewResponse (_rec, SID);
			}
			util.Send(createdHead);
			
		}

        /// <summary>
        /// Create HTTP header for response to Control Point.
        /// Called when request is to renew subscription.
        /// </summary>
        /// <param name="rec">Received message</param>
        /// <param name="uuid">Unique ID for Control Point</param>
        /// <returns></returns>
		private string GetHeadRenewResponse (string rec, string uuid)
		{
			string ResponseHeader = "HTTP/1.1 200 OK\r\n" +
					"SID: uuid:" + uuid + "\r\n" +
					"TIMEOUT: Second-30\r\n\r\n";
			return ResponseHeader;
		}

        /// <summary>
        /// Create HTTP header for response to Control Point.
        /// Called when request is to create a new subscription.
        /// </summary>
        /// <param name="rec">Received message</param>
        /// <param name="guid">A new, unique UUID used from now on, to identify the subscriber</param>
        /// <returns></returns>
		private string GetHeadNewSubscriberResponse(string rec,string guid)
		{
			string ResponseHeader = "HTTP/1.1 200 OK\r\n" +
					"SID: uuid:" + guid + "\r\n" +
					"TIMEOUT: Second-30\r\n\r\n";
			return ResponseHeader;
		}

        /// <summary>
        /// Get UUID from HTTP message.
        /// </summary>
        /// <param name="rec">Received message</param>
        /// <returns>SID</returns>
		private string GetSid (string rec)
		{
			string[] splitter = new string[] {"\r\n"};
			string[] allHeaders = rec.Split (splitter, StringSplitOptions.None);

			string SidHeader ="";
			string[] SidHeaderArray = null;
			string Sid = "";

			foreach (string s in allHeaders) 
			{
				if(s.Contains("SID"))
				{
					SidHeader = s.ToLower();
				}

			}

			if(SidHeader == "")
				return null;

			SidHeaderArray = SidHeader.Split(':');

			Sid = SidHeaderArray[2];
			Sid = Sid.Replace(" ", "");
			return Sid;
		}

        /// <summary>
        /// Get URL from message
        /// </summary>
        /// <param name="rec">received message</param>
        /// <returns>URL from message</returns>
		private string GetURL (string rec)
		{
			string[] splitter = new string[] {"\r\n"}; 
			string[] allHeaders = rec.Split (splitter, StringSplitOptions.None);
			string url ="";

			foreach (string header in allHeaders) {
				if(header.ToLower().Contains("callback"))
				{
					string[] CallBackHeaderArr = header.Split('<');
					url = CallBackHeaderArr[1].Replace(">", "");
				}
			}

			return url;
		}
	}

    /// <summary>
    /// This class will take care of dealing with the order
    /// and respond appropriately over the TCP connection. 
    /// If TCP connection is closed prematurely by control point, 
    /// the senders address will still be in the mesage. (Handle this?)
    /// </summary>
    public interface IOrder
    {
        void execOrder(List<UPnPArg> argList);
    }
     
    /// <summary>
    /// Used by POSTResponder to execute a UPnP Action.
    /// </summary>
    public class Order : IOrder
    {
        private string action = "default";
        private InvokeResponseGen invokeResponseGen;
        private INetworkUtillity util;
        private System.Timers.Timer timer;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="order">Action type</param>
        /// <param name="utillity">Network Utility, used to answer the control point when action has been invoked</param>
        public Order(string order, INetworkUtillity utillity)
        {
            action = order;
            invokeResponseGen = new InvokeResponseGen();
            util = utillity;

            timer = new System.Timers.Timer();
        }

        /// <summary>
        /// Execute the order. 
        /// 
        /// Raises events, that are subcsribed to by inner functionality in the system.
        /// Starts timer on 30 seconds, if CallBack is not raised within that time, the message should respond
        /// to the Control Point with error message.
        /// </summary>
        /// <param name="argList">List of arguments needed to execute the action</param>
        public void execOrder(List<UPnPArg> argList)
        {
            var args = new UPnPEventArgs(argList, action);
            CallBack cb = new CallBack(CallBackFunction);
            EventContainer.RaiseActionEvent(this, args, cb);

            timer.Elapsed += ConnectionTimedOut;
            timer.Interval = 30000; //UPnP Default timeout is 30 seconds
            timer.Enabled = true;
        }

        /// <summary>
        /// Delegate pointing at this function is passed with the ActionEvent raised in the Order's construction
        /// Generates a response and returns it to Control point.
        /// </summary>
        /// <param name="argList">List of OUT arguments in the UPnP Action.
        /// Must be EXACTLY the same order and number, as is stated in the service description</param>
        /// <param name="act">Action Type</param>
        public void CallBackFunction(List<UPnPArg> argList, string act)
        {
            Console.WriteLine("This is callback");
            if (util.IsConnected())
            {
				string response = invokeResponseGen.InvokeResponse(act, argList);

                if(TCPDebug.DEBUG) 
                    Console.WriteLine("invoke answer: \n\r" + response);

                Console.WriteLine("Ready to send in callback");
                util.Send(response);
                util.Close();

                timer.Elapsed -= ConnectionTimedOut;
                timer.Enabled = false;
                timer.Dispose();
            }
        }

        /// <summary>
        /// If connection times out, an error message is created and returned to the Control Point.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">Will be used to specify arguments.</param>
        private void ConnectionTimedOut(object e, ElapsedEventArgs args)
        {
            if (util.IsConnected())
            {
                string act = "TimedOut";
                List<UPnPArg> argList = new List<UPnPArg>();

                string response = invokeResponseGen.InvokeResponse(act, argList);

				if(TCPDebug.DEBUG) 
                	Console.WriteLine("invoke answer: \n\r" + response);

                util.Send(response);
                util.Close();

                timer.Elapsed -= ConnectionTimedOut;
                timer.Enabled = false;
                timer.Dispose();
            }
            
        }
    }
}