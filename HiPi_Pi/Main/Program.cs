using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnP_DvSink.DvWrapper;
using UPnP_DvSource.DvWrapper;

namespace Main
{
    class Program
    {
        private static SinkDevice sink_device;
        private static SourceDevice source_device;

        [STAThread]
        static void Main(string[] args)
        {
            System.Console.WriteLine("HiPi Sink Device. Console applikation");

            //Initiate sink sink_device:
            StartSink();
            StartSource();

            System.Console.WriteLine("Press return to stop device.");
            System.Console.ReadLine();

            sink_device.Stop();
            source_device.Stop();
        }

        public static void StartSink()
        {
            sink_device = new SinkDevice();
            sink_device.Start();
        }

        public static void StartSource()
        {
            source_device = new SourceDevice();
            source_device.Start();
        }
    }
}
