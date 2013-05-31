using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Containers;
using UPnP_Device;

namespace UPnP_Device
{
    /// <summary>
    /// Delegate used to create UPnP events. 
    /// </summary>
    /// <param name="e">sender</param>
    /// <param name="args">List of arguments</param>
    /// <param name="action">Type of action</param>
    /// <param name="cb">CallBack delegate to respond with</param>
    public delegate void UPnPEventDel(object e, List<UPnPArg> args, string action, CallBack cb);

    /// <summary>
    /// Passed with every event send into the system.
    /// This is the delegate used to respond to the UPnP devices.
    /// </summary>
    /// <param name="argList">List of Arguments to return to invoking control point</param>
    /// <param name="action">UPnP Action type</param>
    public delegate void CallBack(List<UPnPArg> argList, string action);

    
    /// <summary>
    /// Event arguments passed along with all UPnP Events
    /// </summary>
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

    /// <summary>
    /// Delegate describing prototype of the ActionEvent, raised by UPnPDevice. 
    /// </summary>
    /// <param name="e">Sender</param>
    /// <param name="args">Argument list</param>
    /// <param name="cb">CallBack Delegate</param>
    public delegate void ActionEventDel(object e, UPnPEventArgs args, CallBack cb);
     
}
