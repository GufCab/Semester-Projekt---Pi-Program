using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device;

namespace UPnP_Device
{
    
    public class EventHandler
    {
        public delegate void UPnPEventDel(object e, List<Tuple<string,string>> args, string action, CallBack cb);

        public event UPnPEventDel UPnPEvent;
       
    }
    
    public delegate void CallBack(List<Tuple<string, string>> argList, string action);

    public class UPnPEventArgs : EventArgs
    {
        public List<Tuple<string, string>> Args { get; private set; }
        public string Action { get; private set; }

        public UPnPEventArgs(List<Tuple<string, string>> argList, string action)
        {
            Args = argList;
            Action = action;
        }
    }

    public delegate void ActionEventDel(object e, UPnPEventArgs args, CallBack cb);
     
}
