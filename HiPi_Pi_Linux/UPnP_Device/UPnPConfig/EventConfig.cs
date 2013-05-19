using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPnP_Device.UPnPConfig
{
    public interface IEventConfig
    {
        void SubscribeToEvents();
        //Event stuff
        //Todo: Implementation needed
    }

    class SinkEventConfig : IEventConfig
    {
        public void SubscribeToEvents()
        {
            //Todo: Subscribtion to all sink events
        }
        //Sink events
        //Todo: Implementation needed
    }

    class SourceEventConfig : IEventConfig
    {
        public void SubscribeToEvents()
        {
            //Todo: Subscribtion to all source events
        }
        //Source events
        //Todo: Implementation needed
    }
}
