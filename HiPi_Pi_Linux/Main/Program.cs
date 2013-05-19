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


namespace Main
{
    class Program
    {
        static void Main()
        {
            IUPnPConfigFactory sinkfact = new SinkFactory();
            IUPnPConfigFactory sourceFact = new SourceFactory();

            UPnP source = new UPnP(sourceFact.CreatePackage());
            UPnP sink = new UPnP(sinkfact.CreatePackage());
            

            PlaybackControl PlayCtrl = new PlaybackControl(sink);
            DBHandle dbHandle = new DBHandle(source);
            

            Console.Read();
        }
        

    }
}
