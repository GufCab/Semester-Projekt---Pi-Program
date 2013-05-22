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
    public interface IRespondStrategy
    {
        void Respond(INetworkUtillity utillity);
    }
    
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

        public void HandleHTTP(object e)
        {
            INetworkUtillity util = new NetworkUtillity((TcpClient) e);

            string rec = util.Receive();

            if(TCPDebug.DEBUG) {Console.WriteLine("New message recieved on TCP.");}
            
            IRespondStrategy respondStrategy = Strat.GetStrategy(rec, _basePath, _pub);

            respondStrategy.Respond(util);
        }

        
    }

    public class GetResponseStrategy
    {
        //Todo: Comments needed:
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

            //Todo: Should handle subscribe.
            //Todo: Also, its a problem that startegy is not set at invalid input
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
