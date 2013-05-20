using System;
using System.ComponentModel;

namespace Containers
{
	public delegate void PropertyChangedDel(object e, string PropertyName);

	public interface IEventChange
	{
		event PropertyChangedDel PropertyChangedEvent;
	}


	public class EventVar
	{

	}
}

