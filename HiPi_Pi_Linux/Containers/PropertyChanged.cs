using System;
using System.ComponentModel;

namespace Containers
{
	public delegate void PropertyChangedDel(UPnPArg args);

	public static class PropertyChangedEvent
	{
		public static event PropertyChangedDel PropEvent;// = delegate { };

		public static void Fire(UPnPArg u)
		{
			PropEvent(u);
		}

		public static bool HasSubscribers()
		{
			return PropEvent != null;
		}
	}


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

