using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UPnP_Device.TCP;

namespace UPnP_Device
{
    public interface IRespondStrategy
    {
        void Respond(INetworkUtillity utillity);
    }
    
    public class TCPHandle
    {
        private GetResponseStrategy Strat = new GetResponseStrategy();

        public TCPHandle()
        {

        }

        public void HandleHTTP(object e)
        {
            INetworkUtillity util = new NetworkUtillity((TcpClient) e);

            string rec = util.Receive();

            Console.WriteLine("New message recieved on TCP.");
            
            

            IRespondStrategy respondStrategy = Strat.GetStrategy(rec);

            respondStrategy.Respond(util);
        }

        
    }

    public class GetResponseStrategy
    {
        private const string GET = "GET / HTTP/1.1";

        public IRespondStrategy GetStrategy(string received)
        {
            IRespondStrategy strategy = null;

            string[] splitter = new string[] { "\r\n" };
            string[] StrArr = received.Split(splitter, StringSplitOptions.None);
            string order = StrArr[0];
            
            Console.WriteLine("Order: " + order);
            Console.WriteLine(received);

            string[] eq = order.Split(' ');

            switch (eq[0])
            {
                case "GET":
                    strategy = new GETResponder(eq[1]);
                    break;
                case "POST":
                    strategy = new POSTResponder(received);
                    break;
                default:
                    Console.WriteLine("Error in Switch-case:");
                    Console.WriteLine(order);
                    break;
            }

            return strategy;
        }
    }

    
}
