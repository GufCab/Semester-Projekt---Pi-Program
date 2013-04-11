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
            System.Console.WriteLine("UPnP .NET Framework Stack");
            System.Console.WriteLine("Device Builder Build#1.0.4144.25068");
            device = new SinkDevice();

            device.Start();

            System.Console.WriteLine("Press return to stop device.");
            System.Console.ReadLine();
            device.Stop();
        }
    }
}
