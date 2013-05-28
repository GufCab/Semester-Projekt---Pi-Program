using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Containers;
using MPlayer;
using UPnP_Device;
using UPnP_Device.UPnPConfig;
using UPnP_Device.XML;
using XMLReader;
using XMLHandler;
using XMLWriter = XMLHandler.XMLWriter;

namespace PlaybackCtrl
{
    /// <summary>
    /// This class manages communication between the UPnP sink, PlayqueueHandler and Audio Player Wrapper
    /// </summary>
    public class PlaybackControl
    {
        private IWrapper Player;
        private IPlayqueueHandler PlayQueueHandler;
        private IUPnP UPnPSink;
        private XMLReader.XMLReader XMLconverter;
		private XMLWriter wr;
		private string _TransportState;


        /// <summary>
        /// Keeps track of evented variables. String contains current state of the playback and broadcasts it whenever it changes.
        /// </summary>
		public string TransportState
		{
			set
			{
				_TransportState = value;

				if (PropertyChangedEvent.HasSubscribers ())
				{
					UPnPArg arg = new UPnPArg ("TransportState", _TransportState);
					PropertyChangedEvent.Fire (arg);
				}
			}
			get {return _TransportState; }
		}
		//public event PropertyChangedDel propEvent;

        /// <summary>
        /// Constructor. Creates Audio Player Wrapper, XMLreader and XMLwriter and subscribes to wrapper and UPnP Sink
        /// </summary>
        /// <param name="sink">UPnP Sink</param>
        /// <param name="pqhandl">Playqueue Handler</param>
        public PlaybackControl(IUPnP sink, IPlayqueueHandler pqhandl)
        {
            UPnPSink = sink;
            Player = new MPlayerWrapper();
			PlayQueueHandler = pqhandl; 
            XMLconverter = new XMLReader.XMLReader();
			wr = new XMLWriter();
            SubscribeToWrapper();
            SubscribeToSink();
			_TransportState = "STOPPED";
        }

        /// <summary>
        /// Gets next Track in playqueue and passes it to Audio Player Wrapper
        /// </summary>
        private void Next()
        {
			TransportState = "TRANSITIONING";
            
			var myTrack = PlayQueueHandler.GetNextTrack();
            Player.PlayTrack(myTrack.Path);

			TransportState = "PLAYING";
        }


        /// <summary>
        /// Gets previous Track in playqueue and passes it to Audio Player Wrapper
        /// </summary>
        private void Prev()
        {
			TransportState = "TRANSITIONING";
            ITrack myTrack = PlayQueueHandler.GetPrevTrack();
            Player.PlayTrack(myTrack.Path);
			TransportState = "PLAYING";
        }

        /// <summary>
        /// Gets next Track in playqueue and passes it to Audio Player Wrapper
        /// </summary>
        private void Play ()
		{
			if (!Player.GetPaused())
			{
				var myTrack = PlayQueueHandler.GetNextTrack ();
				Player.PlayTrack (myTrack.Path);
			}
        }

        /// <summary>
        /// Toggles pause.
        /// </summary>
        private void Pause ()
		{
			Player.PauseTrack ();
			if (Player.GetPaused ()) 
			{
				TransportState = "STOPPED";
			}
			else
			{
				TransportState = "PLAYING";
			}
        }

        /// <summary>
        /// Immediately starts playing the Track contained in param.
        /// </summary>
        /// <param name="retValRef">UPnPArg containing a Track</param>
        private void SetCurrentURI(ref List<UPnPArg> retValRef)
        {
			TransportState = "TRANSITIONING";
            Player.PlayTrack(retValRef[1].ArgVal);
			TransportState = "PLAYING";
        }

        /// <summary>
        /// Adds Track contained in param to playqueue.
        /// </summary>
        /// <param name="retValRef">UPnPArg containing a Track</param>
        private void AddToPlayQueue(ref List<UPnPArg> retValRef)
        {
            List<ITrack> myTrackList = XMLconverter.itemReader(retValRef[1].ArgVal); //Converts xml file to Track
            PlayQueueHandler.AddToPlayQueue(myTrackList[0]);
        }

        /// <summary>
        /// Plays Track at specific position in queue
        /// </summary>
        /// <param name="index">integer specifying position to play at</param>
        private void PlayAt(int index)
        {
            var myTrack = PlayQueueHandler.GetTrack(index);
            Player.PlayTrack(myTrack.Path);
        }

        /// <summary>
        /// Adds Track contained in param to specific position in playqueue
        /// </summary>
        /// <param name="retValRef">UPnPArg containing a Track</param>
        /// <param name="index">Integer specifying desired position in playqueue</param>
        private void AddToPlayQueue(ref List<UPnPArg> retValRef, int index)
        {
            List<ITrack> myTrackList = XMLconverter.itemReader(retValRef[1].ArgVal);
            PlayQueueHandler.AddToPlayQueue(myTrackList[0], index);
        }

        /// <summary>
        /// Removes Track at specified position in playqueue.
        /// </summary>
        /// <param name="retValRef">UPnPArg containing the position to remove from</param>
        private void RemoveFromPlayQueue(ref List<UPnPArg> retValRef)
        {
			PlayQueueHandler.RemoveFromPlayQueue(Convert.ToInt32(retValRef[1].ArgVal));
        }

        /// <summary>
        /// Returns position in playback.
        /// </summary>
        /// <returns>String containing position</returns>
        private string GetPos()
        {
            return Player.GetPosition();
        }

        /// <summary>
        /// Sets position in playback, used for skipping in Track
        /// </summary>
        /// <param name="retValRef">UPnPArg containing desired position</param>
        private void SetPos(ref List<UPnPArg> retValRef)
        {
            Player.SetPosition(Convert.ToInt32(retValRef[1].ArgVal));
			retValRef = null;
        }

        /// <summary>
        /// Returns current volume of Audio Player Wrapper.
        /// </summary>
        /// <returns>String indicating volume</returns>
        private string GetVol()
        {
            return Player.GetVolume();
        }

        /// <summary>
        /// Sets volume of Audio Player Wrapper.
        /// </summary>
        /// <param name="retValRef">UPnPArg containing desired volume</param>
        private void SetVol(ref List<UPnPArg> retValRef)
        {
            Player.SetVolume(Convert.ToInt32(retValRef[2].ArgVal));
			Console.WriteLine("\nInside Setvol!");
        }


        /// <summary>
        /// Subscribes to Wrapper 
        /// </summary>
        private void SubscribeToWrapper()
        {
            Player.EOF_Event += NewSongHandler;
        }

        /// <summary>
        /// Subscribes to UPnP Sink
        /// </summary>
        private void SubscribeToSink()
        {
            UPnPSink.ActionEvent += UPnPHandler;
        }

        /// <summary>
        /// Called whenever an UPnP event is received, reads the Action argument and responds appropriately
        /// </summary>
        /// <param name="e">Unused param to comply with standard</param>
        /// <param name="args">UPnPEventArgs containing desired action and params for the action</param>
        /// <param name="cb">Callback function to call at end of function</param>
        private void UPnPHandler(object e, UPnPEventArgs args, CallBack cb)
        {
            string action = args.Action;
			bool CallCB = true;
			Console.WriteLine(">> Inside UPnPHandler: " + action);
            List<UPnPArg> returnVal = args.Args;

            if (action != "Browse")
            {
			    switch (args.Action)
                {
                case "Play":
                    Play();
					returnVal = null;
					break;

                case "Next":
                    Next();
					returnVal = null;
					break;

                case "Previous":
                    Prev();
					returnVal = null;
                    break;

                case "Pause":
                    Pause();
					returnVal = null;
                    break;

                case "SetNextAVTransportURI":
                    AddToPlayQueue(ref returnVal);
					returnVal = null;
                    break;

                case "SetAVTransportURI":
                    SetCurrentURI(ref returnVal);
					returnVal = null;
                    break;

                case "Remove":
                    RemoveFromPlayQueue(ref returnVal);
					returnVal = null;
                    break;

                case "SetVolume":
					SetVol(ref returnVal);
                    returnVal = null;
                    break;

                case "SetPosition":
                    SetPos(ref returnVal);
					returnVal = null;
                    break;

                case "GetVolume":
					string  vol = GetVol();
					returnVal = new List<UPnPArg>(){new UPnPArg("CurrentVolume", vol)};
					break;

                case "GetPosition":
                   	returnVal = CreatePosArgs(returnVal);
                    break;

				case "GetCurrentTrack":
					returnVal = new List<UPnPArg>();
					returnVal.Add(new UPnPArg("CurrentTrack",  wr.ConvertITrackToXML(new List<ITrack>() {PlayQueueHandler.GetCurrentTrack()})));
					returnVal.Add (new UPnPArg("PlayQueueChanged", PlayQueueHandler.PlayQueueChanged));
					break;

				case "GetIPAddress":
					CallCB = false;
					break;

                default:
                    Console.WriteLine("PlaybackControl class switchcase default");
                    break;
                }

			if(CallCB)
			{
		        Console.WriteLine("PlaybackCtrl ready for callback");
		        cb(returnVal, args.Action);
			}
        }


    }
        //[Todo]:Describe this
 		private List<UPnPArg> CreatePosArgs (List<UPnPArg> inArgs)
		{
			List<UPnPArg> createdArgs = new List<UPnPArg>();
			createdArgs.Add(new UPnPArg("CurrentPosition", GetPos()));

			//createdArgs.Add(new UPnPArg("Duration", PlayQueueHandler.GetCurrentTrack().Duration));
			createdArgs.Add(new UPnPArg("Duration", "100"));

			return createdArgs;
		}

        /// <summary>
        /// Called when Audio Player Wrapper completes playback of a Track. Starts playback of next Track in playqueue if there are more, otherwise playback ends.
        /// </summary>
        /// <param name="e">Unused param to comply with standard</param>
        /// <param name="args">Unused param to comply with standard</param>
		private void NewSongHandler (object e, EventArgs args)
        {
            //Plays next song. If playqueue is empty it stops playing.
            if (PlayQueueHandler.GetNumberOfTracks() > PlayQueueHandler.GetCurrentTrackIndex())
            {
                Next();
            }
        }
    }
}
