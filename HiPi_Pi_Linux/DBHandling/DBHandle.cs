using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Containers;
using UPnP_Device;
using XMLHandler;
using PlaybackCtrl;

/// <summary>
/// Namespace for all DBClasses.
/// </summary>

namespace DBClasses
{
    /// <summary>
    /// Class acting as layer between UPnP communication layer 
    /// and Database access layer
    /// </summary>
    public class DBHandle
    {
        private IUPnP _sourceDevice;
        private XMLWriter _dbXmlWriter;
        private Dictionary<string, IDBStrategy> _strategies;
        
		private DBLookup _dbLookup;
		private IPlayqueueHandler _PQHandler;

		//public event PropertyChangedDel propEvent;

        /// <summary>
        /// Constructor 
        /// Creates associations between the class, The UPnP device and the playqueue handler.
        /// </summary>
        /// <param name="sourceDevice">UPnP Source device communicating with control points</param>
        /// <param name="pqhandl">Handler containing and managing the playqueue</param>
        public DBHandle(IUPnP sourceDevice, IPlayqueueHandler pqhandl)
        {
            _sourceDevice = sourceDevice;
            _sourceDevice.ActionEvent += _sourceDevice_ActionEvent;

            _dbXmlWriter = new XMLWriter();
            _dbLookup = new DBLookup();

			_PQHandler = pqhandl;

            CreateDictionary();
        }

        /// <summary>
        /// Handler for ActionEvents.
        /// Reacts to ActionEvents and sees if data is valid.
        /// 
        /// If action is valid it is handled.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args"></param>
        /// <param name="cb"></param>
        void _sourceDevice_ActionEvent (object e, UPnPEventArgs args, CallBack cb)
		{
			
			if (args.Action == "Browse")
			{
                Console.WriteLine("DB Handle Action: " + args.Action);
				IDBStrategy strat = _strategies [args.Action];
				strat.Handle (args.Args, cb, _dbXmlWriter, _PQHandler);
			}
		    else if (args.Action == "GetIPAddress")
			{
				Console.WriteLine ("GetIPAddress");
				HandleGetIP(cb);
			}
            
        }

        /// <summary>
        /// Create dictionary. 
        /// Written for later implementation of more functionality.
        /// </summary>
        public void CreateDictionary()
        {
            _strategies = new Dictionary<string, IDBStrategy>();
            _strategies.Add("Browse", new BrowseStrat(_dbLookup));
        }

        /// <summary>
        /// Handle GetIP requests and return the IP of the source adress.
        /// </summary>
        /// <param name="cb">CallBack delegate to respond to UPnP device</param>
		public void HandleGetIP(CallBack cb)
		{
			UPnPArg p = new UPnPArg("IPAddress", _sourceDevice.GetIP());
			Console.WriteLine ("Arg: " + p.ArgVal);

			List<UPnPArg> argList = new List<UPnPArg>();
			argList.Add (p);
			Console.WriteLine (">> UPnPARGLIST:");
			Console.WriteLine (argList[0].ArgVal);
			cb(argList, "GetIPAddress");
		}
    }

    /// <summary>
    /// Strategy interface used for handling incoming database related actions.
    /// </summary>
    public interface IDBStrategy
    {
        void Handle(List<UPnPArg> args, CallBack cb, XMLWriter writer, IPlayqueueHandler pqhandl);
    }

    /// <summary>
    /// Concrete strategy class implementing the IDBStrategy interface.
    /// 
    /// This class uses a DBLookup to actually access the Database.
    /// </summary>
    public class BrowseStrat : IDBStrategy
    {
        private DBLookup _dbLookup;

        /// <summary>
        /// Constructor
        /// Sets DBLookup reference.
        /// </summary>
        /// <param name="lookup"></param>
        public BrowseStrat(DBLookup lookup)
        {
            _dbLookup = lookup;
        }

        /// <summary>
        /// Function for executing the strategy.
        /// 
        /// Strategy implements getting the "all" container id. 
        /// It also implements the "playqueue" container id.
        /// This is a dirty fix and will be revised to take full advantage of the strategy pattern.
        /// 
        /// </summary>
        /// <param name="args">Arguments to pass to the inner functionality</param>
        /// <param name="cb">callback to answer back to the UPnP Device</param>
        /// <param name="writer">XMLWriter for writing tracks from the database to sendable to XML</param>
        /// <param name="pqhandl">The playqueue handler containing the playlist</param>
        public void Handle (List<UPnPArg> args, CallBack cb, XMLWriter writer, IPlayqueueHandler pqhandl)
		{
			Console.WriteLine ("Browse Was called (BrowseStrat)");
			List<UPnPArg> retArgs = null;
            

			string containerId = GetContainerID (args);

			if (containerId == "all")
			{
				retArgs = new List<UPnPArg>();
				List<ITrack> containingList = _dbLookup.Browse (containerId);
				int NumberReturned = containingList.Count;

				string retVal = writer.ConvertITrackToXML (containingList);

				retArgs.Add (new UPnPArg ("Result", retVal));
				//retArgs.Add (new UPnPArg("Result", "<DIDL-Lite> music stuff </DIDL-Lite>"));
				retArgs.Add (new UPnPArg ("NumberReturned", NumberReturned.ToString ()));
				retArgs.Add (new UPnPArg ("TotalMatches", NumberReturned.ToString ()));
				retArgs.Add (new UPnPArg ("UpdateID", "12"));

				cb (retArgs, "Browse");
			}
			else if (containerId == "playqueue") 
			{
				retArgs = new List<UPnPArg>();
                List<ITrack> lis = new List<ITrack>();

			    string retVal = writer.ConvertITrackToXML(pqhandl.GetQueue());
                
				retArgs.Add (new UPnPArg ("Result", retVal));
				retArgs.Add (new UPnPArg ("NumberReturned", pqhandl.GetQueue().Count.ToString()));
				retArgs.Add (new UPnPArg ("TotalMatches", pqhandl.GetQueue().Count.ToString()));
				retArgs.Add (new UPnPArg ("UpdateID", "12"));

				cb(retArgs, "Browse");
			}
			else
			{

				cb (retArgs, "Browse");
			}
            
        }

        /// <summary>
        /// Get the container id from the sent arguments from control point. 
        /// </summary>
        /// <param name="args">IN arguments send from control point</param>
        /// <returns>Container id as string</returns>
        private string GetContainerID(List<UPnPArg> args)
        {
            foreach (var upnPArg in args)
            {
                if (upnPArg.ArgName == "ObjectID")
                    return upnPArg.ArgVal;
            }
            return "BadArgs";
        }
    }
}
