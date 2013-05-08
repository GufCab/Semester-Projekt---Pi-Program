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
            Console.WriteLine("new message recieved on TCP");
            
            string[] splitter = new string[] {"\r\n"};

            var StrArr = rec.Split(splitter, StringSplitOptions.None);

            IRespondStrategy respondStrategy = Strat.GetStrategy(StrArr[0]);

            respondStrategy.Respond(util);
        }

        
    }

    public class GetResponseStrategy
    {
        private const string GET = "GET / HTTP/1.1";

        public IRespondStrategy GetStrategy(string order)
        {
            IRespondStrategy strategy = null;
            Console.WriteLine("Order: " + order);

            string[] eq = order.Split(' ');
            
            //TOdo: Head "GET / HTTP/1.0" should be handle and ignored

            switch (eq[0])
            {
                case "GET":
                    strategy = new GETResponder(eq[1]);
                    break;
                case "POST":
                    //Todo: return post value
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
