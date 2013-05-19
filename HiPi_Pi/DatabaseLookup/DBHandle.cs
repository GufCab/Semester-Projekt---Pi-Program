using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Containers;
using UPnP_Device;
using XMLHandler;

namespace DBClasses
{
    public class DBHandle
    {
        private IUPnP _sourceDevice;
        private XMLWriterPi _dbXmlWriter;
        private Dictionary<string, IDBStrategy> _strategies;
        private DbLookup _dbLookup;

        public DBHandle(IUPnP sourceDevice)
        {
            _sourceDevice = sourceDevice;
            _sourceDevice.ActionEvent += _sourceDevice_ActionEvent;

            _dbXmlWriter = new XMLWriterPi();
            _dbLookup = new DbLookup();

            CreateDictionary();
        }

        void _sourceDevice_ActionEvent(object e, UPnPEventArgs args, CallBack cb)
        {
            IDBStrategy strat = _strategies[args.Action];
            strat.Handle(args.Args, cb, _dbXmlWriter);
        }

        public void CreateDictionary()
        {
            _strategies.Add("Browse", new BrowseStrat(_dbLookup));
        }
    }

    public interface IDBStrategy
    {
        void Handle(List<UPnPArg> args, CallBack cb, XMLWriterPi writer);
    }

    public class BrowseStrat : IDBStrategy
    {
        private DbLookup _dbLookup;

        public BrowseStrat(DbLookup lookup)
        {
            _dbLookup = lookup;
        }

        public void Handle(List<UPnPArg> args, CallBack cb, XMLWriterPi writer)
        {
            Console.WriteLine("Browse Was called (BrowseStrat)");
            List<UPnPArg> retArgs = new List<UPnPArg>();
            

            string containerId = GetContainerID(args);

            if (containerId == "BadArgs")
            {
                cb(retArgs, containerId);
            }
            else
            {
                List<ITrack> containingList = _dbLookup.Browse(containerId);
                int NumberReturned = containingList.Count;

                string retVal = writer.ConvertITrackToXML(containingList);

                retArgs.Add(new UPnPArg("Result", retVal));
                retArgs.Add(new UPnPArg("NumberReturned", NumberReturned.ToString()));
                retArgs.Add(new UPnPArg("TotalMatches", NumberReturned.ToString()));

                cb(retArgs, "Browse");
            }
            
        }

        private string GetContainerID(List<UPnPArg> args)
        {
            foreach (var upnPArg in args)
            {
                if (upnPArg.ArgName == "ContainerID")
                    return upnPArg.ArgVal;
            }
            return "BadArgs";
        }
    }
}
