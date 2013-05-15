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
            Console.WriteLine("Entry point");
            
            UPnPMain main = new UPnPMain();

            Console.ReadLine();
        }
    }
}
