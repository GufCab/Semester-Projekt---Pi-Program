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

        public delegate void PauseOrderHandler(object e, UPnPEventArgs args);

        //Har her lavet en triviel handler, så der aldrig er null subscribers. 
        public static event PauseOrderHandler PauseEvent = delegate { };
        public static void RaisePauseEvent(object e, UPnPEventArgs args)
        {
            PauseEvent(e, args);
        }

        public delegate void PreviousOrderHandler(object e, UPnPEventArgs args);

        //Har her lavet en triviel handler, så der aldrig er null subscribers. 
        public static event PreviousOrderHandler PreviousEvent = delegate { };
        public static void RaisePreviousEvent(object e, UPnPEventArgs args)
        {
            PreviousEvent(e, args);
        }

        public delegate void SetAVTransportURIOrderHandler(object e, UPnPEventArgs args);

        //Har her lavet en triviel handler, så der aldrig er null subscribers. 
        public static event SetAVTransportURIOrderHandler SetAVTransportURIEvent = delegate { };
        public static void RaiseSetAVTransportURIEvent(object e, UPnPEventArgs args)
        {
            SetAVTransportURIEvent(e, args);
        }
    }
}
