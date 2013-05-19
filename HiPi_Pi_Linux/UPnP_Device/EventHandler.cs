using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_Device;

namespace UPnP_Device
{
    
    public delegate void UPnPEventDel(object e, List<UPnPArg> args, string action, CallBack cb);

    public delegate void CallBack(List<UPnPArg> argList, string action);

    public class UPnPArg
    {
        public string ArgName { get; private set; }
        public string ArgVal { get; private set; }

        public UPnPArg(string argName, string argVal)
        {
            ArgName = argName;
            ArgVal = argVal;
        }
    }

    public class UPnPEventArgs : EventArgs
    {
        public List<UPnPArg> Args { get; private set; }
        public string Action { get; private set; }

        public UPnPEventArgs(List<UPnPArg> argList, string action)
        {
            Args = argList;
            Action = action;
        }
    }

    public delegate void ActionEventDel(object e, UPnPEventArgs args, CallBack cb);
     
}
