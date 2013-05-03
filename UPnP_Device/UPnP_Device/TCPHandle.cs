using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UPnP_Device
{
    class TCPHandle
    {
        public TCPHandle(string message)
        {
               msgHandleThread(message);
        }

        private void msgHandleThread(string i)
        {
            //handle message
        }
    }
}
