// UPnP .NET Framework Device Stack, Device Module
// Device Builder Build#1.0.4144.25068

using System;
using OpenSource.UPnP;
//using Playback;
using UPnP_DvSink.SinkStacks;

namespace UPnP_DvSink.DvWrapper
{
    internal interface ISinkDevice
    {
        #region
        void Start();
        void Stop();
        void AVTransport_GetCurrentTransportActions(System.UInt32 InstanceID, out System.String Actions);
        void AVTransport_GetDeviceCapabilities(System.UInt32 InstanceID, out DvAVTransport.Enum_PossiblePlaybackStorageMedia PlayMedia, out DvAVTransport.Enum_PossibleRecordStorageMedia RecMedia, out DvAVTransport.Enum_PossibleRecordQualityModes RecQualityModes);
        void AVTransport_GetMediaInfo(System.UInt32 InstanceID, out System.UInt32 NrTracks, out System.String MediaDuration, out System.String CurrentURI, out System.String CurrentURIMetaData, out System.String NextURI, out System.String NextURIMetaData, out DvAVTransport.Enum_PlaybackStorageMedium PlayMedium, out DvAVTransport.Enum_RecordStorageMedium RecordMedium, out DvAVTransport.Enum_RecordMediumWriteStatus WriteStatus);
        void AVTransport_GetPositionInfo(System.UInt32 InstanceID, out System.UInt32 Track, out System.String TrackDuration, out System.String TrackMetaData, out System.String TrackURI, out System.String RelTime, out System.String AbsTime, out System.Int32 RelCount, out System.Int32 AbsCount);
        void AVTransport_GetTransportInfo(System.UInt32 InstanceID, out DvAVTransport.Enum_TransportState CurrentTransportState, out DvAVTransport.Enum_TransportStatus CurrentTransportStatus, out DvAVTransport.Enum_TransportPlaySpeed CurrentSpeed);
        void AVTransport_GetTransportSettings(System.UInt32 InstanceID, out DvAVTransport.Enum_CurrentPlayMode PlayMode, out DvAVTransport.Enum_CurrentRecordQualityMode RecQualityMode);
        void AVTransport_Next(System.UInt32 InstanceID);
        void AVTransport_Pause(System.UInt32 InstanceID);
        void AVTransport_Play(System.UInt32 InstanceID, DvAVTransport.Enum_TransportPlaySpeed Speed);
        void AVTransport_Previous(System.UInt32 InstanceID);
        void AVTransport_Seek(System.UInt32 InstanceID, DvAVTransport.Enum_A_ARG_TYPE_SeekMode Unit, System.String Target);
        void AVTransport_SetAVTransportURI(System.UInt32 InstanceID, System.String CurrentURI, System.String CurrentURIMetaData);
        void AVTransport_SetPlayMode(System.UInt32 InstanceID, DvAVTransport.Enum_CurrentPlayMode NewPlayMode);
        void AVTransport_Stop(System.UInt32 InstanceID);
        void ConnectionManager_GetCurrentConnectionIDs(out System.String ConnectionIDs);
        void ConnectionManager_GetCurrentConnectionInfo(System.Int32 ConnectionID, out System.Int32 RcsID, out System.Int32 AVTransportID, out System.String ProtocolInfo, out System.String PeerConnectionManager, out System.Int32 PeerConnectionID, out DvConnectionManager.Enum_A_ARG_TYPE_Direction Direction, out DvConnectionManager.Enum_A_ARG_TYPE_ConnectionStatus Status);
        void ConnectionManager_GetProtocolInfo(out System.String Source, out System.String Sink);
        void RenderingControl_GetMute(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.Boolean CurrentMute);
        void RenderingControl_GetVolume(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.UInt16 CurrentVolume);
        void RenderingControl_GetVolumeDB(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.Int16 CurrentVolume);
        void RenderingControl_GetVolumeDBRange(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.Int16 MinValue, out System.Int16 MaxValue);
        void RenderingControl_ListPresets(System.UInt32 InstanceID, out DvRenderingControl.Enum_PresetNameList CurrentPresetNameList);
        void RenderingControl_SelectPreset(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_PresetName PresetName);
        void RenderingControl_SetMute(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, System.Boolean DesiredMute);
        void RenderingControl_SetVolume(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, System.UInt16 DesiredVolume);
        #endregion
    }

    /// <summary>
	/// Summary description for SampleDevice.
	/// </summary>
	class SinkDevice
    {
		private UPnPDevice device;

	    private DvAVTransport AVTransport;
	    private DvConnectionManager ConnectionManager;
	    private DvRenderingControl RenderingControl;

        //private Playback.PlaybackCtrl player;
		
		public SinkDevice()
		{
		    //player = new PlaybackCtrl();
            Console.WriteLine("0");
			device = UPnPDevice.CreateRootDevice(1800,1.0,"\\");
            Console.WriteLine("1");
            #region Device information. Can be accessed by Control Point
            device.FriendlyName = "HiPi Sink";
			device.Manufacturer = "Casper";
			device.ManufacturerURL = "http://www.plutinosoft.com";
			device.ModelName = "HiPi";
			device.ModelDescription = "HiPi UPnP Renderer (Sink)";
			device.ModelNumber = "";
			device.HasPresentation = false;
			device.DeviceURN = "urn:schemas-upnp-org:device:MediaRenderer:1";
            #endregion
            Console.WriteLine("2");
            //Add AVTrans to service:
			AVTransport = new DvAVTransport();
            #region AVTransport wrapper. Enables us to implement in methods below
            AVTransport.External_GetCurrentTransportActions = new DvAVTransport.Delegate_GetCurrentTransportActions(AVTransport_GetCurrentTransportActions);
			AVTransport.External_GetDeviceCapabilities = new DvAVTransport.Delegate_GetDeviceCapabilities(AVTransport_GetDeviceCapabilities);
			AVTransport.External_GetMediaInfo = new DvAVTransport.Delegate_GetMediaInfo(AVTransport_GetMediaInfo);
			AVTransport.External_GetPositionInfo = new DvAVTransport.Delegate_GetPositionInfo(AVTransport_GetPositionInfo);
			AVTransport.External_GetTransportInfo = new DvAVTransport.Delegate_GetTransportInfo(AVTransport_GetTransportInfo);
			AVTransport.External_GetTransportSettings = new DvAVTransport.Delegate_GetTransportSettings(AVTransport_GetTransportSettings);
			AVTransport.External_Next = new DvAVTransport.Delegate_Next(AVTransport_Next);
			AVTransport.External_Pause = new DvAVTransport.Delegate_Pause(AVTransport_Pause);
			AVTransport.External_Play = new DvAVTransport.Delegate_Play(AVTransport_Play);
			AVTransport.External_Previous = new DvAVTransport.Delegate_Previous(AVTransport_Previous);
			AVTransport.External_Seek = new DvAVTransport.Delegate_Seek(AVTransport_Seek);
			AVTransport.External_SetAVTransportURI = new DvAVTransport.Delegate_SetAVTransportURI(AVTransport_SetAVTransportURI);
			AVTransport.External_SetPlayMode = new DvAVTransport.Delegate_SetPlayMode(AVTransport_SetPlayMode);
			AVTransport.External_Stop = new DvAVTransport.Delegate_Stop(AVTransport_Stop);
            #endregion
            device.AddService(AVTransport);
            Console.WriteLine("3");

            //Add CM to service:
            ConnectionManager = new DvConnectionManager();
            #region ConnectionManager wrapper. Enables us to implement in methods below
            ConnectionManager.External_GetCurrentConnectionIDs = new DvConnectionManager.Delegate_GetCurrentConnectionIDs(ConnectionManager_GetCurrentConnectionIDs);
			ConnectionManager.External_GetCurrentConnectionInfo = new DvConnectionManager.Delegate_GetCurrentConnectionInfo(ConnectionManager_GetCurrentConnectionInfo);
			ConnectionManager.External_GetProtocolInfo = new DvConnectionManager.Delegate_GetProtocolInfo(ConnectionManager_GetProtocolInfo);
            #endregion
            device.AddService(ConnectionManager);
            Console.WriteLine("4");
			
            //Add RC to service:
			RenderingControl = new DvRenderingControl();
            #region RenderingControl wrapper. Enables us to implement in methods below
            RenderingControl.External_GetMute = new DvRenderingControl.Delegate_GetMute(RenderingControl_GetMute);
			RenderingControl.External_GetVolume = new DvRenderingControl.Delegate_GetVolume(RenderingControl_GetVolume);
			RenderingControl.External_GetVolumeDB = new DvRenderingControl.Delegate_GetVolumeDB(RenderingControl_GetVolumeDB);
			RenderingControl.External_GetVolumeDBRange = new DvRenderingControl.Delegate_GetVolumeDBRange(RenderingControl_GetVolumeDBRange);
			RenderingControl.External_ListPresets = new DvRenderingControl.Delegate_ListPresets(RenderingControl_ListPresets);
			RenderingControl.External_SelectPreset = new DvRenderingControl.Delegate_SelectPreset(RenderingControl_SelectPreset);
			RenderingControl.External_SetMute = new DvRenderingControl.Delegate_SetMute(RenderingControl_SetMute);
			RenderingControl.External_SetVolume = new DvRenderingControl.Delegate_SetVolume(RenderingControl_SetVolume);
            #endregion
            device.AddService(RenderingControl);
            Console.WriteLine("5");

			// Setting the initial value of evented variables
			AVTransport.Evented_LastChange = "Sample String";
			ConnectionManager.Evented_SourceProtocolInfo = "Sample String";
			ConnectionManager.Evented_SinkProtocolInfo = "Sample String";
			ConnectionManager.Evented_CurrentConnectionIDs = "Sample String";
			RenderingControl.Evented_LastChange = "Sample String";
		}
		
		public void Start()
		{
			device.StartDevice();
		}
		
		public void Stop()
		{
			device.StopDevice();
		}

        //AVTransport functions:
        #region AVTransport methods
        public void AVTransport_GetCurrentTransportActions(System.UInt32 InstanceID, out System.String Actions)
		{
			Actions = "Sample String";
			Console.WriteLine("AVTransport_GetCurrentTransportActions(" + InstanceID.ToString() + ")"); 
		}
		
		public void AVTransport_GetDeviceCapabilities(System.UInt32 InstanceID, out DvAVTransport.Enum_PossiblePlaybackStorageMedia PlayMedia, out DvAVTransport.Enum_PossibleRecordStorageMedia RecMedia, out DvAVTransport.Enum_PossibleRecordQualityModes RecQualityModes)
		{
			PlayMedia = DvAVTransport.Enum_PossiblePlaybackStorageMedia.NONE;
			RecMedia = DvAVTransport.Enum_PossibleRecordStorageMedia.NOT_IMPLEMENTED;
			RecQualityModes = DvAVTransport.Enum_PossibleRecordQualityModes.NOT_IMPLEMENTED;
			Console.WriteLine("AVTransport_GetDeviceCapabilities(" + InstanceID.ToString() + ")");
		}
		
		public void AVTransport_GetMediaInfo(System.UInt32 InstanceID, out System.UInt32 NrTracks, out System.String MediaDuration, out System.String CurrentURI, out System.String CurrentURIMetaData, out System.String NextURI, out System.String NextURIMetaData, out DvAVTransport.Enum_PlaybackStorageMedium PlayMedium, out DvAVTransport.Enum_RecordStorageMedium RecordMedium, out DvAVTransport.Enum_RecordMediumWriteStatus WriteStatus)
		{
			NrTracks = 0;
			MediaDuration = "Sample String";
			CurrentURI = "Sample String";
			CurrentURIMetaData = "Sample String";
			NextURI = "Sample String";
			NextURIMetaData = "Sample String";
			PlayMedium = DvAVTransport.Enum_PlaybackStorageMedium.NONE;
			RecordMedium = DvAVTransport.Enum_RecordStorageMedium.NOT_IMPLEMENTED;
			WriteStatus = DvAVTransport.Enum_RecordMediumWriteStatus.NOT_IMPLEMENTED;
			Console.WriteLine("AVTransport_GetMediaInfo(" + InstanceID.ToString() + ")");
		}
		
		public void AVTransport_GetPositionInfo(System.UInt32 InstanceID, out System.UInt32 Track, out System.String TrackDuration, out System.String TrackMetaData, out System.String TrackURI, out System.String RelTime, out System.String AbsTime, out System.Int32 RelCount, out System.Int32 AbsCount)
		{
			Track = 0;
			TrackDuration = "Sample String";
			TrackMetaData = "Sample String";
			TrackURI = "Sample String";
			RelTime = "Sample String";
			AbsTime = "Sample String";
			RelCount = 0;
			AbsCount = 0;
			Console.WriteLine("AVTransport_GetPositionInfo(" + InstanceID.ToString() + ")");
		}
		
		public void AVTransport_GetTransportInfo(System.UInt32 InstanceID, out DvAVTransport.Enum_TransportState CurrentTransportState, out DvAVTransport.Enum_TransportStatus CurrentTransportStatus, out DvAVTransport.Enum_TransportPlaySpeed CurrentSpeed)
		{
			CurrentTransportState = DvAVTransport.Enum_TransportState.STOPPED;
			CurrentTransportStatus = DvAVTransport.Enum_TransportStatus.OK;
			CurrentSpeed = DvAVTransport.Enum_TransportPlaySpeed._1;
			Console.WriteLine("AVTransport_GetTransportInfo(" + InstanceID.ToString() + ")");
		}
		
		public void AVTransport_GetTransportSettings(System.UInt32 InstanceID, out DvAVTransport.Enum_CurrentPlayMode PlayMode, out DvAVTransport.Enum_CurrentRecordQualityMode RecQualityMode)
		{
			PlayMode = DvAVTransport.Enum_CurrentPlayMode.NORMAL;
			RecQualityMode = DvAVTransport.Enum_CurrentRecordQualityMode.NOT_IMPLEMENTED;
			Console.WriteLine("AVTransport_GetTransportSettings(" + InstanceID.ToString() + ")");
		}
		
		public void AVTransport_Next(System.UInt32 InstanceID)
		{
			Console.WriteLine("AVTransport_Next(" + InstanceID.ToString() + ")");

           // player.Next();
		}
		
		public void AVTransport_Pause(System.UInt32 InstanceID)
		{
			Console.WriteLine("AVTransport_Pause(" + InstanceID.ToString() + ")");

		  //  player.Pause();
		}
		
		public void AVTransport_Play(System.UInt32 InstanceID, DvAVTransport.Enum_TransportPlaySpeed Speed)
		{
			Console.WriteLine("AVTransport_Play(" + InstanceID.ToString() + Speed.ToString() + ")");

		   // player.Play();
		}
		
		public void AVTransport_Previous(System.UInt32 InstanceID)
		{
			Console.WriteLine("AVTransport_Previous(" + InstanceID.ToString() + ")");

           // player.Prev();
		}
		
		public void AVTransport_Seek(System.UInt32 InstanceID, DvAVTransport.Enum_A_ARG_TYPE_SeekMode Unit, System.String Target)
		{
			Console.WriteLine("AVTransport_Seek(" + InstanceID.ToString() + Unit.ToString() + Target.ToString() + ")");
		}
		
		public void AVTransport_SetAVTransportURI(System.UInt32 InstanceID, System.String CurrentURI, System.String CurrentURIMetaData)
		{
			//Console.WriteLine("AVTransport_SetAVTransportURI(" + InstanceID.ToString() + CurrentURI.ToString() + CurrentURIMetaData.ToString() + ")");
		    Console.WriteLine("Called setURI");

		  //  player.AddToPlayQueue(CurrentURI);
		}
		
		public void AVTransport_SetPlayMode(System.UInt32 InstanceID, DvAVTransport.Enum_CurrentPlayMode NewPlayMode)
		{
			Console.WriteLine("AVTransport_SetPlayMode(" + InstanceID.ToString() + NewPlayMode.ToString() + ")");
		}
		
		public void AVTransport_Stop(System.UInt32 InstanceID)
		{
			Console.WriteLine("AVTransport_Stop(" + InstanceID.ToString() + ")");
		}
        #endregion

        //ConnectionManager functions:
        #region ConnectionManager methods
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

        //RenderingControl service:
        #region RenderingControl
        public void RenderingControl_GetMute(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.Boolean CurrentMute)
		{
			CurrentMute = false;
			Console.WriteLine("RenderingControl_GetMute(" + InstanceID.ToString() + Channel.ToString() + ")");
		}
		
		public void RenderingControl_GetVolume(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.UInt16 CurrentVolume)
		{
			CurrentVolume = 0;
			Console.WriteLine("RenderingControl_GetVolume(" + InstanceID.ToString() + Channel.ToString() + ")");
		}
		
		public void RenderingControl_GetVolumeDB(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.Int16 CurrentVolume)
		{
			CurrentVolume = 0;
			Console.WriteLine("RenderingControl_GetVolumeDB(" + InstanceID.ToString() + Channel.ToString() + ")");
		}
		
		public void RenderingControl_GetVolumeDBRange(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, out System.Int16 MinValue, out System.Int16 MaxValue)
		{
			MinValue = 0;
			MaxValue = 0;
			Console.WriteLine("RenderingControl_GetVolumeDBRange(" + InstanceID.ToString() + Channel.ToString() + ")");
		}
		
		public void RenderingControl_ListPresets(System.UInt32 InstanceID, out DvRenderingControl.Enum_PresetNameList CurrentPresetNameList)
		{
			CurrentPresetNameList = DvRenderingControl.Enum_PresetNameList.FACTORYDEFAULTS;
			Console.WriteLine("RenderingControl_ListPresets(" + InstanceID.ToString() + ")");
		}
		
		public void RenderingControl_SelectPreset(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_PresetName PresetName)
		{
			Console.WriteLine("RenderingControl_SelectPreset(" + InstanceID.ToString() + PresetName.ToString() + ")");
		}
		
		public void RenderingControl_SetMute(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, System.Boolean DesiredMute)
		{
			Console.WriteLine("RenderingControl_SetMute(" + InstanceID.ToString() + Channel.ToString() + DesiredMute.ToString() + ")");
		}
		
		public void RenderingControl_SetVolume(System.UInt32 InstanceID, DvRenderingControl.Enum_A_ARG_TYPE_Channel Channel, System.UInt16 DesiredVolume)
		{
			Console.WriteLine("RenderingControl_SetVolume(" + InstanceID.ToString() + Channel.ToString() + DesiredVolume.ToString() + ")");
        }
        #endregion
    }
}

