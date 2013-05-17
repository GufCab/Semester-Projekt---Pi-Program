using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UPnP_Device.UDP;
using UPnP_Device.UPnPConfig;
using UPnP_Device.XML;

namespace UPnP_Device
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entry point");
            
            //UPnPMain main = new UPnPMain();

            IUPnPConfig upnpConfig = new UPnPConfig.UPnPConfig();
            upnpConfig.LoadConfig("config/ConfigHiPiMediaRenderer.txt");
            //var writer = new XMLWriterSink();
            //writer.GenDeviceDescription(upnpConfig);
            //writer.GenServiceDescription();
            //writer.genServiceXmlAVTransport();

            XMLServicesConfig xmlServicesConfig = new XMLServicesConfig();
            xmlServicesConfig.LoadConfig("config/ConfigAVTransportService.txt");

            //Console.WriteLine(xmlServicesConfig._functions[0]);

            foreach (var s in xmlServicesConfig._functions)
            {
                Console.WriteLine(s.functionName);
                Console.WriteLine("------------");
                foreach (var s1 in s.arguments)
                {
                    Console.WriteLine(s1.argumentName);
                    Console.WriteLine(s1.direction);
                    Console.WriteLine(s1.relatedStateVariable);
                    Console.WriteLine();
                }
            }

            Console.ReadLine();
        }
    }
}
