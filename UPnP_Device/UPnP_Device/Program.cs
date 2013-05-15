using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UPnP_Device.UDP;

namespace UPnP_Device
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ready, set ...");
            Console.ReadLine();
            Console.WriteLine("GOO!!!");
            
            UPnPMain main = new UPnPMain();
            XMLWriter1 x = new XMLWriter1();
            x.genGETxml();
            x.genServiceXml();
            /*
            XMLReader reader = new XMLReader();
            List<Tuple<string, string>> s = new List<Tuple<string, string>>();
            string ss = "";
            s = reader.ReadArguments(ss, "Play");

            int i = 0;
            foreach (Tuple<string, string> tuple in s)
            {
                Console.WriteLine(s[i]);
                ++i;
            }
            */

            Console.ReadLine();
        }
    }
}
