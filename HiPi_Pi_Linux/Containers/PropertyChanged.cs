using System;
using System.ComponentModel;

namespace Containers
{
	public delegate void PropertyChangedDel(UPnPArg args);

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

	public interface IEventChange
	{
		event PropertyChangedDel PropertyChangedEvent;
	}


	public class EventVar
	{

	}
}

