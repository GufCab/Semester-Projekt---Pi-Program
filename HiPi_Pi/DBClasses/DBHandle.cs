using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPnP_Device;

namespace DBClasses
{
    public class DBLookup
    {
        
    }

    public class DBHandle
    {
        private IUPnP _sourceDevice;
        private DBXmlWriter _dbXmlWriter;
        private Dictionary<string, IDBStrategy> _strategies;
        //private DBLookup _dbLookup;

        public DBHandle(IUPnP sourceDevice)
        {
            _sourceDevice = sourceDevice;
            _sourceDevice.ActionEvent += _sourceDevice_ActionEvent;

            _dbLookup = new DBLookup();

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
        void Handle(List<UPnPArg> args, CallBack cb, DBXmlWriter writer);
    }

    public class BrowseStrat : IDBStrategy
    {
        private DBLookup _dbLookup;

        public BrowseStrat(DBLookup lookup)
        {
            _dbLookup = lookup;
        }

        public void Handle(List<UPnPArg> args, CallBack cb, DBXmlWriter writer)
        {
            Console.WriteLine("Browse Was called (BrowseStrat)");
            List<UPnPArg> retArgs = new List<UPnPArg>();
            

            string containerId = GetContainerID(args);

            if (containerId == "BadArgs")
            {
                cb(retArgs, containerId);
                return;
            }

            var ContainingList = _dbLookup.Browse(containerId);
            

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
