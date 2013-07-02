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

/// <summary>
/// The HiPi_Pi namespace is the general namespace containing main functionality
/// </summary
namespace HiPi_Pi
{}

/// <summary>
/// This is the namespace for the main thread
/// </summary>
namespace Main
{
	/// <summary>
	/// This is the Program main function.
	/// Runs in the Main thread, and is the entrypoint of the program
	/// </summary>
    class Program
    {
        static void Main()
        {
            //Program entrypoint
            IUPnPConfigFactory sinkfact = new SinkFactory();
            IUPnPConfigFactory sourceFact = new SourceFactory();

			UPnP sink = new UPnP(sinkfact.CreatePackage());
            UPnP source = new UPnP(sourceFact.CreatePackage());
            
            
			IPlayqueueHandler pqhandl = new PlayqueueHandler();

            PlaybackControl PlayCtrl = new PlaybackControl(sink, pqhandl);
            DBHandle dbHandle = new DBHandle(source, pqhandl);
            
			AbstractFileSenderServer Fserver = new FileSenderServer.FileSenderServer();

			Console.WriteLine ("==============================================================");
			Console.WriteLine ("Welcome to the HiPi Server solution");
			Console.WriteLine ("All UPnP devices SHOULD work, but that's probably not the case");
			Console.WriteLine ("Feel free to get our Windows application to get all features");
			Console.WriteLine ("Enjoy! ;)");
			Console.WriteLine ("==============================================================");


            Console.Read();
        }
        

    }
}
