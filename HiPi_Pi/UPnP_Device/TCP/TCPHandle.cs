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

        //Why???
        //Todo: Remove, maybe???
        public TCPHandle(string BasePath)
        {
            _basePath = BasePath;
        }

        public void HandleHTTP(object e)
        {
            INetworkUtillity util = new NetworkUtillity((TcpClient) e);

            string rec = util.Receive();

            if(TCPDebug.DEBUG) {Console.WriteLine("New message recieved on TCP.");}
            
            IRespondStrategy respondStrategy = Strat.GetStrategy(rec, _basePath);

            respondStrategy.Respond(util);
        }

        
    }

    public class GetResponseStrategy
    {
        //Todo: Comments needed:
        public IRespondStrategy GetStrategy(string received, string BasePath)
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
                    strategy = new SubscribeStrategy();
                    break;
                default:
                    strategy = new BadRequestStrategy();
                    break;
            }

            return strategy;
        }
    }

    
}
