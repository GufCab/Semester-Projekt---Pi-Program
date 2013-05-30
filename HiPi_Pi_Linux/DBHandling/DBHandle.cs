﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Containers;
using UPnP_Device;
using XMLHandler;
using PlaybackCtrl;

namespace DBClasses
{
    public class DBHandle
    {
        private IUPnP _sourceDevice;
        private XMLWriter _dbXmlWriter;
        private Dictionary<string, IDBStrategy> _strategies;
        
		private DBLookup _dbLookup;
		private IPlayqueueHandler _PQHandler;

		//public event PropertyChangedDel propEvent;

        public DBHandle(IUPnP sourceDevice, IPlayqueueHandler pqhandl)
        {
            _sourceDevice = sourceDevice;
            _sourceDevice.ActionEvent += _sourceDevice_ActionEvent;

            _dbXmlWriter = new XMLWriter();
            _dbLookup = new DBLookup();

			_PQHandler = pqhandl;

            CreateDictionary();
        }

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

        public void CreateDictionary()
        {
            _strategies = new Dictionary<string, IDBStrategy>();
            _strategies.Add("Browse", new BrowseStrat(_dbLookup));
        }

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

    public interface IDBStrategy
    {
        void Handle(List<UPnPArg> args, CallBack cb, XMLWriter writer, IPlayqueueHandler pqhandl);
    }

    public class BrowseStrat : IDBStrategy
    {
        private DBLookup _dbLookup;

        public BrowseStrat(DBLookup lookup)
        {
            _dbLookup = lookup;
        }

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
