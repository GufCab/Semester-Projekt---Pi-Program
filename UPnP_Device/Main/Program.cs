using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UPnPConfigFactory;
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

            IUPnPConfigFactory sinkfact = new SinkFactory();

            UPnP u = new UPnP(sinkfact.CreatePackage());

            
            #region xmlTestStuff Outcommented stuff
            /*
            IUPnPConfig upnpConfig = new UPnPConfig.UPnPConfig();
            upnpConfig.LoadConfig("config/ConfigHiPiMediaRenderer.txt");
            var writer = new XMLWriter();
            List<string> sList = new List<string>();
            sList.Add("config/ConfigServiceAVTransport.txt");
            sList.Add("config/ConfigServiceRenderingControl.txt");

            XMLServicesConfig xmlServicesConfig = new XMLServicesConfig(sList);

            /*
            XMLServicesConfig xmlServicesConfig = new XMLServicesConfig();

            xmlServicesConfig.LoadConfig("config/ConfigServiceAVTransport.txt");
            writer.GenServiceDescription("AVTransport", xmlServicesConfig._functions);
            
            xmlServicesConfig = new XMLServicesConfig();

            xmlServicesConfig.LoadConfig("config/ConfigServiceRenderingControl.txt");
            writer.GenServiceDescription("RenderingControl", xmlServicesConfig._functions);
            
            foreach (var s in xmlServicesConfig._functions)
            {
                Console.WriteLine(s.functionName);
                Console.WriteLine("------------");
                foreach (var s1 in s.arguments)
                {
                    Console.WriteLine(s1.argumentName);
                    Console.WriteLine(s1.direction);
                    Console.WriteLine(s1.relatedStateVariable);
                    Console.WriteLine(s1.sendEventAttribute);
                    Console.WriteLine(s1.dataType);
                    Console.WriteLine();
                }
            }
            */
            #endregion

            u.ActionEvent += u_ActionEvent;
            Console.ReadLine();
        }

        static void u_ActionEvent(object e, UPnPEventArgs args, CallBack cb)
        {
            //cb()
        }
    }
}
