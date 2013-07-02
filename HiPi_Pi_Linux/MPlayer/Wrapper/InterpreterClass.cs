using System;

namespace MPlayer
{
	/// <summary>
	/// Only has limited features. Should be removed at future refactoring
	/// </summary>
	static public class InterpreterClass
	{
		static public string[] DataToArray (string data)
		{
			return data.Split('=');
		}
	}
}

