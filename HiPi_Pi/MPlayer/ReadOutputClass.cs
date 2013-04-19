using System;

namespace MPlayer
{
    public static class ReadOutputClass
    {
        public static string[] GetData(string data)
        {
            var resultArray = data.Split('=');

            return resultArray;
        }

    }
}