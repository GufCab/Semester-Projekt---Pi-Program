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
        void Respond(NetworkStream networkStream);
    }
    
    public class TCPHandle
    {
        public TCPHandle()
        {

        }

        public void HandleHTTP(object e)
        {
            var objArray = (object[])e;
            NetworkStream networkStream = (NetworkStream)objArray[0];
            string firstHTTP = (string)objArray[1];
            IRespondStrategy respondStrategy = GetResponseStrategy.GetStrategy(firstHTTP);



        }

        
    }

    public static class GetResponseStrategy
    {
        private const string GET = "GET / HTTP/1.1";

        public static IRespondStrategy GetStrategy(string order)
        {
            IRespondStrategy strategy = null;

            switch (order)
            {
                case GET:
                    strategy = new GetResponse();
                    break;
            }

            return strategy;
        }
    }

    
}
