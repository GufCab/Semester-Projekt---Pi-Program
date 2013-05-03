using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device.UPnP
{
    public class UPnPEvents
    {
        public delegate void PauseOrder(object e, UPnPEventArgs args);

        public delegate void SetTransportURIOrder(object e, UPnPEventArgs args);

        public delegate void PlayOrder(object e, UPnPEventArgs args);
    }    
}
