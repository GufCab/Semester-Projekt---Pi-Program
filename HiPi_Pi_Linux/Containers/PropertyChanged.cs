using System;
using System.ComponentModel;

namespace Containers
{
	
	public delegate void PropertyChangedDel(UPnPArg args);
	/// <summary>
	/// Method to invoke at property changed event
	/// </summary>
	public static class PropertyChangedEvent
	{
		public static event PropertyChangedDel PropEvent = delegate { };

		public static void Fire(UPnPArg u)
		{
			PropEvent(u);
		}

		public static bool HasSubscribers()
		{
			return PropEvent != null;
		}
	}

	/// <summary>
	/// Container of arguments received and send with UPnP
	/// </summary>
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

	/// <summary>
	/// Interface for the Property Changed event.
	/// </summary>
	public interface IEventChange
	{
		event PropertyChangedDel PropertyChangedEvent;
	}

/*
	public class EventVar
	{

	}
*/
}

