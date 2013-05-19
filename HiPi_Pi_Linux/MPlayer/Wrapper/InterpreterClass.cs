using System;

namespace MPlayer
{
	static public class InterpreterClass
	{
		static public string[] DataToArray (string data)
		{
			return data.Split('=');
		}
	}
}

