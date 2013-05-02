// UPnP .NET Framework Device Stack, Device Module
// Device Builder Build#1.0.4144.25068

using System;
using OpenSource.UPnP;
using UPnP_DvSource.SourceStacks;

namespace UPnP_DvSource.DvWrapper
{
	/// <summary>
	/// Summary description for SourceDevice.
	/// </summary>
	public class SourceDevice
	{
		private UPnPDevice device;

	    private DvConnectionManager ConnectionManager;
	    private DvContentDirectory ContentDirectory;
		public SourceDevice()
		{
			device = UPnPDevice.CreateRootDevice(1800,1.0,"\\");

            //Device infomation should be changed at some point:
            #region Device information. Can be accessed by Control Point
            device.FriendlyName = "LS-CHL761: LinkStation";
			device.Manufacturer = "BUFFALO INC.";
			device.ManufacturerURL = "http://www.buffalotech.com/";
			device.ModelName = "LinkStation";
			device.ModelDescription = "LinkStation";
			device.ModelNumber = "4.4";
			device.HasPresentation = false;
			device.DeviceURN = "urn:schemas-upnp-org:device:MediaServer:1";
            #endregion

            //Add ConnectionManager to service:
            ConnectionManager = new DvConnectionManager();
            #region ConnectionManager wrapper. Allowes us to implement methods below
            ConnectionManager.External_GetCurrentConnectionIDs = new SourceStacks.DvConnectionManager.Delegate_GetCurrentConnectionIDs(ConnectionManager_GetCurrentConnectionIDs);
			ConnectionManager.External_GetCurrentConnectionInfo = new SourceStacks.DvConnectionManager.Delegate_GetCurrentConnectionInfo(ConnectionManager_GetCurrentConnectionInfo);
			ConnectionManager.External_GetProtocolInfo = new SourceStacks.DvConnectionManager.Delegate_GetProtocolInfo(ConnectionManager_GetProtocolInfo);
            #endregion
            device.AddService(ConnectionManager);

            //Add ContentDirectory to service:
			ContentDirectory = new DvContentDirectory();
            #region ContentDirectory wrapper. Allowes us to implement methods below
            ContentDirectory.External_Browse = new SourceStacks.DvContentDirectory.Delegate_Browse(ContentDirectory_Browse);
			ContentDirectory.External_GetSearchCapabilities = new SourceStacks.DvContentDirectory.Delegate_GetSearchCapabilities(ContentDirectory_GetSearchCapabilities);
			ContentDirectory.External_GetSortCapabilities = new SourceStacks.DvContentDirectory.Delegate_GetSortCapabilities(ContentDirectory_GetSortCapabilities);
			ContentDirectory.External_GetSystemUpdateID = new SourceStacks.DvContentDirectory.Delegate_GetSystemUpdateID(ContentDirectory_GetSystemUpdateID);
			ContentDirectory.External_Search = new SourceStacks.DvContentDirectory.Delegate_Search(ContentDirectory_Search);
			ContentDirectory.External_X_BrowseByLetter = new SourceStacks.DvContentDirectory.Delegate_X_BrowseByLetter(ContentDirectory_X_BrowseByLetter);
            #endregion
            device.AddService(ContentDirectory);
			
			// Setting the initial value of evented variables
			ConnectionManager.Evented_SourceProtocolInfo = "Sample String";
			ConnectionManager.Evented_SinkProtocolInfo = "Sample String";
			ConnectionManager.Evented_CurrentConnectionIDs = "Sample String";
			ContentDirectory.Evented_TransferIDs = "Sample String";
			ContentDirectory.Evented_ContainerUpdateIDs = "Sample String";
			ContentDirectory.Evented_SystemUpdateID = 0;
		}
		
		public void Start()
		{
			device.StartDevice();
		}
		
		public void Stop()
		{
			device.StopDevice();
		}

        //ConnectionManager Wrapper methods. Functionality can be implemented below:
        #region ConnectionManager methods:
        public void ConnectionManager_GetCurrentConnectionIDs(out System.String ConnectionIDs)
		{
			ConnectionIDs = "Sample String";
			Console.WriteLine("ConnectionManager_GetCurrentConnectionIDs(" + ")");
		}
		
		public void ConnectionManager_GetCurrentConnectionInfo(System.Int32 ConnectionID, out System.Int32 RcsID, out System.Int32 AVTransportID, out System.String ProtocolInfo, out System.String PeerConnectionManager, out System.Int32 PeerConnectionID, out DvConnectionManager.Enum_A_ARG_TYPE_Direction Direction, out DvConnectionManager.Enum_A_ARG_TYPE_ConnectionStatus Status)
		{
			RcsID = 0;
			AVTransportID = 0;
			ProtocolInfo = "Sample String";
			PeerConnectionManager = "Sample String";
			PeerConnectionID = 0;
			Direction = DvConnectionManager.Enum_A_ARG_TYPE_Direction.INPUT;
			Status = DvConnectionManager.Enum_A_ARG_TYPE_ConnectionStatus.OK;
			Console.WriteLine("ConnectionManager_GetCurrentConnectionInfo(" + ConnectionID.ToString() + ")");
		}
		
		public void ConnectionManager_GetProtocolInfo(out System.String Source, out System.String Sink)
		{
			Source = "Sample String";
			Sink = "Sample String";
			Console.WriteLine("ConnectionManager_GetProtocolInfo(" + ")");
		}
        #endregion

        //ContentDirectory Wrapper methods. Functionality can be implemented below:
        #region ContentDirectory methods
        public void ContentDirectory_Browse(System.String ObjectID, DvContentDirectory.Enum_A_ARG_TYPE_BrowseFlag BrowseFlag, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String Result, out System.UInt32 NumberReturned, out System.UInt32 TotalMatches, out System.UInt32 UpdateID)
		{
			Result = "Sample String";
			NumberReturned = 0;
			TotalMatches = 0;
			UpdateID = 0;
			Console.WriteLine("ContentDirectory_Browse(" + ObjectID.ToString() + BrowseFlag.ToString() + Filter.ToString() + StartingIndex.ToString() + RequestedCount.ToString() + SortCriteria.ToString() + ")");
		}
		
		public void ContentDirectory_GetSearchCapabilities(out System.String SearchCaps)
		{
			SearchCaps = "Sample String";
			Console.WriteLine("ContentDirectory_GetSearchCapabilities(" + ")");
		}
		
		public void ContentDirectory_GetSortCapabilities(out System.String SortCaps)
		{
			SortCaps = "Sample String";
			Console.WriteLine("ContentDirectory_GetSortCapabilities(" + ")");
		}
		
		public void ContentDirectory_GetSystemUpdateID(out System.UInt32 Id)
		{
			Id = 0;
			Console.WriteLine("ContentDirectory_GetSystemUpdateID(" + ")");
		}
		
		public void ContentDirectory_Search(System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String Result, out System.UInt32 NumberReturned, out System.UInt32 TotalMatches, out System.UInt32 UpdateID)
		{
			Result = "Sample String";
			NumberReturned = 0;
			TotalMatches = 0;
			UpdateID = 0;
			Console.WriteLine("ContentDirectory_Search(" + ContainerID.ToString() + SearchCriteria.ToString() + Filter.ToString() + StartingIndex.ToString() + RequestedCount.ToString() + SortCriteria.ToString() + ")");
		}
		
		public void ContentDirectory_X_BrowseByLetter(System.String ObjectID, DvContentDirectory.Enum_A_ARG_TYPE_BrowseFlag BrowseFlag, System.String Filter, System.String StartingLetter, System.UInt32 RequestedCount, System.String SortCriteria, out System.String Result, out System.UInt32 NumberReturned, out System.UInt32 TotalMatches, out System.UInt32 UpdateID, out System.UInt32 StartingIndex)
		{
			Result = "Sample String";
			NumberReturned = 0;
			TotalMatches = 0;
			UpdateID = 0;
			StartingIndex = 0;
			Console.WriteLine("ContentDirectory_X_BrowseByLetter(" + ObjectID.ToString() + BrowseFlag.ToString() + Filter.ToString() + StartingLetter.ToString() + RequestedCount.ToString() + SortCriteria.ToString() + ")");
        }
        #endregion

    }
}

