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
        void Respond(TCPUtillity utillity);
    }
    
    public class TCPHandle
    {
        public TCPHandle()
        {

        }

        public void HandleHTTP(object e)
        {
            var util = (TCPUtillity) e;
            string rec = util.TCPRecieve();
            string[] splitter = new string[] {"\r\n"};

            var StrArr = rec.Split(splitter, StringSplitOptions.None);

            IRespondStrategy respondStrategy = GetResponseStrategy.GetStrategy(StrArr[0]);

            respondStrategy.Respond(util);

        }

        
    }

    public static class GetResponseStrategy
    {
        private const string GET = "GET / HTTP/1.1";

        public static IRespondStrategy GetStrategy(string order)
        {
            IRespondStrategy strategy = null;
            Console.WriteLine("Order: " + order);

            //TOdo: Head "GET / HTTP/1.0" should be handle and ignored

            switch (order)
            {
                case GET:
                    strategy = new GetResponse();
                    break;
                default:
                    Console.WriteLine("Error in Switch-case");
                    break;
            }

            return strategy;
        }
    }

    
}
