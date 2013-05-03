using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device.UDP
{
    public class MulticastReceiver
    {
        private string _UUID;
        private int _cacheexpire;
        private string _localip;
        private int _tcpport;

        public MulticastReceiver(string uuid, int cacheexpire, string localip, int tcpport)
        {
            _UUID = uuid;
            _cacheexpire = cacheexpire;
            _localip = localip;
            _tcpport = tcpport;    
        }



    }
}
