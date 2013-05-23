using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using DBClasses;
using UPnPConfigFactory;
using UPnP_Device;
using PlaybackCtrl;
using XMLHandler;
using FileSenderServer;


namespace Main
{
    class Program
    {
        static void Main()
        {
            IUPnPConfigFactory sinkfact = new SinkFactory();
            IUPnPConfigFactory sourceFact = new SourceFactory();

			UPnP sink = new UPnP(sinkfact.CreatePackage());
            UPnP source = new UPnP(sourceFact.CreatePackage());
            
            
			IPlayqueueHandler pqhandl = new PlayqueueHandler();

            PlaybackControl PlayCtrl = new PlaybackControl(sink, pqhandl);
            DBHandle dbHandle = new DBHandle(source, pqhandl);
            
			Server Fserver = new Server();


            Console.Read();
        }
        

    }
}
