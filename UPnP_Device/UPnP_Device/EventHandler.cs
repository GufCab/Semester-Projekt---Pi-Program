using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device
{
    public class EventHandler
    {
        public delegate void UPnPEventDel(object e, List<Tuple<string,string>> args, string action);

        public event UPnPEventDel UPnPEvent;

        public void SubscribeToEvents()
        {
            TCP.EventContainer.PlayEvent += new TCP.EventContainer.PlayOrderHandler(ListenToPlay);
            TCP.EventContainer.NextEvent += new TCP.EventContainer.NextOrderHandler(ListenToNext);
            TCP.EventContainer.StopEvent += new TCP.EventContainer.StopOrderHandler(ListenToStop);
            TCP.EventContainer.PauseEvent += new TCP.EventContainer.PauseOrderHandler(ListenToPause);
            TCP.EventContainer.PreviousEvent += new TCP.EventContainer.PreviousOrderHandler(ListenToPrevious);
            TCP.EventContainer.SetAVTransportURIEvent += new TCP.EventContainer.SetAVTransportURIOrderHandler(ListenToSetAVTransport);
        }

        private void ListenToPlay(object e, UPnPEventArgs args)
        {
            UPnPEvent(null, args.Args, args.Action);
        }

        private void ListenToNext(object e, UPnPEventArgs args)
        {
            UPnPEvent(null, args.Args, args.Action);
        }

        private void ListenToStop(object e, UPnPEventArgs args)
        {
            UPnPEvent(null, args.Args, args.Action);
        }

        private void ListenToPause(object e, UPnPEventArgs args)
        {
            UPnPEvent(null, args.Args, args.Action);
        }

        private void ListenToPrevious(object e, UPnPEventArgs args)
        {
            UPnPEvent(null, args.Args, args.Action);
        }

        private void ListenToSetAVTransport(object e, UPnPEventArgs args)
        {
            UPnPEvent(null, args.Args, args.Action);
        }
    }

    public class UPnPEventArgs : EventArgs
    {
        public List<Tuple<string, string>> Args { get; private set; }
        public string Action { get; private set; }
        public delegate void CallBack(List<Tuple<string,string>> args);

        public event CallBack CallBackEvt;

        public UPnPEventArgs(List<Tuple<string, string>> argList, string action)
        {
            Args = argList;
            Action = action;
        }
    }
}
