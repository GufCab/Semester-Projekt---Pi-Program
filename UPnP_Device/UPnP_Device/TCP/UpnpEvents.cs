using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Todo: move this entire file + class to main scope
//Todo: So everyone knows the required event types.
namespace UPnP_Device.TCP
{
    public static class EventContainer
    {
        public delegate void TcpActionDel(object e, UPnPEventArgs args, CallBack cb);

        //Har her lavet en triviel handler, så der aldrig er null subscribers. 
        public static event TcpActionDel TcpActionEvent = delegate { };
        public static void RaiseActionEvent(object e, UPnPEventArgs args, CallBack cb)
        {
            TcpActionEvent(e, args, cb);
        }
    }

    

}
