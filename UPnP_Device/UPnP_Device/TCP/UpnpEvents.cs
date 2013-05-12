using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device.TCP
{

    /// <summary>
    /// More or less a message distribution system.
    /// Main UPNP class will subscribeto all these events 
    /// and propagate them to PlayCtrl.
    /// </summary>
    public static class EventContainer
    {
        public delegate void PlayOrderHandler(object e, UPnPEventArgs args);

        //Har her lavet en triviel handler, så der aldrig er null subscribers. 
        public static event PlayOrderHandler PlayEvent = delegate { };
        public static void RaisePlayEvent(object e, UPnPEventArgs args)
        {
            PlayEvent(e, args);
        }

        public delegate void StopOrderHandler(object e, UPnPEventArgs args);

        //Har her lavet en triviel handler, så der aldrig er null subscribers. 
        public static event StopOrderHandler StopEvent = delegate { };
        public static void RaiseStopEvent(object e, UPnPEventArgs args)
        {
            StopEvent(e, args);
        }

        public delegate void NextOrderHandler(object e, UPnPEventArgs args);

        //Har her lavet en triviel handler, så der aldrig er null subscribers. 
        public static event NextOrderHandler NextEvent = delegate { };
        public static void RaiseNextEvent(object e, UPnPEventArgs args)
        {
            NextEvent(e, args);
        }
    }
}
