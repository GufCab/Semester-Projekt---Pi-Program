using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UPnP_Device.TCP;
using UPnP_Device.UPnPConfig;

namespace UPnP_Device
{
    /// <summary>
    /// Interface for Respond Strategy.
    /// The respons strategy is used in a form of the GoF
    /// Strategy Pattern, by TCPHandle to appropriately 
    /// handle and respond to messages 
    /// </summary>
    public interface IRespondStrategy
    {
        void Respond(INetworkUtillity utillity);
    }
    
    /// <summary>
    /// Main class used for handling TCP/HTTP requests
    /// This class is set up with a basepath to determine, compile time, 
    /// the base path where the decription files are stored/written.
    /// </summary>
    public class TCPHandle
    {
        private GetResponseStrategy Strat = new GetResponseStrategy();
        private string _basePath;
		private Publisher _pub;

        //Why???
        //Todo: Remove, maybe???
        public TCPHandle(string BasePath, IIpConfig ipConf)
        {
            _basePath = BasePath;
			_pub = new Publisher(ipConf);
        }

        /// <summary>
        /// For each incoming TCP connection, TCPServer starts
        /// a new thread with this function as run function.
        /// 
        /// This function creates a NetworkUtility with the argument e.
        /// This is used for handling the TCP communication with the Control Point.
        /// The function uses GetResponseStrategy.GetStrategy to get a fitting
        /// strategy to Respond to and handle the incoming message.
        /// This is an implementation of the GoF strategy pattern.
        /// </summary>
        /// <param name="e">
        /// TcpClient containing the accepted socket connection
        /// with control point. 
        /// Sent as object type.
        /// </param>
        public void HandleHTTP(object e)
        {
            INetworkUtillity util = new NetworkUtillity((TcpClient) e);

            string rec = util.Receive();

            if(TCPDebug.DEBUG) {Console.WriteLine("New message recieved on TCP.");}
            
            IRespondStrategy respondStrategy = Strat.GetStrategy(rec, _basePath, _pub);

            respondStrategy.Respond(util);
        }

        
    }
    /// <summary>
    /// Utility class used for getting a fitting strategy to 
    /// respond to the Control Point.
    /// </summary>
    public class GetResponseStrategy
    {
        /// <summary>
        /// Function returning the strategy, based on order type.
        /// </summary>
        /// <param name="received">The entire HTTP message sent with the request</param>
        /// <param name="BasePath">Path to directory containing the desc.xml files for descriptions</param>
        /// <param name="pub">The instance of publisher containing all subscribers, used for eventing</param>
        /// <returns></returns>
        public IRespondStrategy GetStrategy(string received, string BasePath, Publisher pub)
        {
            IRespondStrategy strategy = null;

            string[] splitter = new string[] { "\r\n" };
            string[] StrArr = received.Split(splitter, StringSplitOptions.None);
            string order = StrArr[0];

            if (TCPDebug.DEBUG)
            {
                Console.WriteLine("Order: " + order);
                Console.WriteLine(received);
            }

            string[] eq = order.Split(' ');

            
            switch (eq[0])
            {
                case "GET":
                    strategy = new GETResponder(BasePath + eq[1]);
                    break;
                case "POST":
                    strategy = new POSTResponder(received);
                    break;
                case "SUBSCRIBE":
					
					strategy = new SubscribeResponder(pub,received);
                    break;
                default:
                    Console.WriteLine("Error in Switch-case:");
					Console.WriteLine (eq[0]);
                    Console.WriteLine(order);
                    break;
            }

            return strategy;
        }
    }

    
}
