using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UPnP_DvSink.DvWrapper;

namespace UPnP_DvSink
{
    class Program
    {
        private static SinkDevice device;

        [STAThread]
        static void Main(string[] args)
        {
            System.Console.WriteLine("HiPi Sink Device. Console applikation");
            
            //Initiate sink device:
            device = new SinkDevice();

            device.Start();

            System.Console.WriteLine("Press return to stop device.");
            System.Console.ReadLine();
            device.Stop();
        }
    }
}
