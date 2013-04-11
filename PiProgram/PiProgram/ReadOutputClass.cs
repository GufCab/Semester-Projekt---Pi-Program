using System;

//Find de ordrer der kommer ud af mplayer
#define
#define

namespace PiProgram
{
    public static class ReadOutputClass
    {
        public static string FindOutOrderType(string mplayerOutput)
        {

            //ToDo Nested if statements with string.Contains
            return "";
        }

        public static string GetData(string data)
        {
            string orderType = FindOutOrderType(data);
            string outPutData = GetDataFromString(orderType, data);

            return "";
        }

        private static string GetDataFromString(string typeOfOrder, string streamData)
        {
            //ToDO String.Contains stuff..
            return "";
        }
    }
}