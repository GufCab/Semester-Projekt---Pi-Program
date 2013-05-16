using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UPnP_Device.UDP;
using UPnP_Device.XML;

namespace UPnP_Device
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entry point");
            
            UPnPMain main = new UPnPMain();

            /*
            var writer = new XMLWriterSink();
            writer.GenDeviceDescription();
            writer.GenServiceDescription();
            */

            Console.ReadLine();
        }
    }
}
