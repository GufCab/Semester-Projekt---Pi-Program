using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPnPConfigFactory;
using UPnP_Device;
using PlaybackCtrl;

namespace Main
{
    class Program
    {
        static void Main()
        {
            IUPnPConfigFactory sinkfact = new SinkFactory();

            UPnP u = new UPnP(sinkfact.CreatePackage());

            //PlaybackControl PlayCtrl = new PlaybackControl(u);
            
    
        }
        

    }
}
