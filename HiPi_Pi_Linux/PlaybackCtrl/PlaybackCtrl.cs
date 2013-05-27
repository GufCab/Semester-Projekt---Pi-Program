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

namespace PlaybackCtrl
{
    public class PlaybackControl
    {
        private IWrapper Player;
        private IPlayqueueHandler PlayQueueHandler;
        private IUPnP UPnPSink;
        private XMLReader1 XMLconverter;
		private XMLWriterPi wr;
		private string _TransportState;

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

        public PlaybackControl(IUPnP sink, IPlayqueueHandler pqhandl)
        {
            UPnPSink = sink;
            Player = new MPlayerWrapper();
			PlayQueueHandler = pqhandl; 
            XMLconverter = new XMLReader1();
			wr = new XMLWriterPi();
            SubscribeToWrapper();
            SubscribeToSink();
			_TransportState = "STOPPED";
        }

        private void Next()
        {
			TransportState = "TRANSITIONING";
            
			var myTrack = PlayQueueHandler.GetNextTrack();
            Player.PlayTrack(myTrack.Path);

			TransportState = "PLAYING";
        }

        private void Prev()
        {
			TransportState = "TRANSITIONING";
            ITrack myTrack = PlayQueueHandler.GetPrevTrack();
            Player.PlayTrack(myTrack.Path);
			TransportState = "PLAYING";
        }

        private void Play ()
		{
			if (!Player.GetPaused())
			{
				var myTrack = PlayQueueHandler.GetNextTrack ();
				Player.PlayTrack (myTrack.Path);
			}
        }

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

			/*
			if (PropertyChangedEvent.HasSubscribers())
			{
				UPnPArg arg = new UPnPArg("TransportState", TransportState);
				PropertyChangedEvent.Fire (arg);
			}
			*/
        }

        private void SetCurrentURI(ref List<UPnPArg> retValRef)
        {
			TransportState = "TRANSITIONING";
            Player.PlayTrack(retValRef[1].ArgVal);
			TransportState = "PLAYING";
        }

        private void AddToPlayQueue(ref List<UPnPArg> retValRef)
        {
            List<ITrack> myTrackList = XMLconverter.itemReader(retValRef[1].ArgVal); //Converts xml file to Track
            PlayQueueHandler.AddToPlayQueue(myTrackList[0]);
        }


        private void PlayAt(int index)
        {
            var myTrack = PlayQueueHandler.GetTrack(index);
            Player.PlayTrack(myTrack.Path);
        }


        private void AddToPlayQueue(ref List<UPnPArg> retValRef, int index)
        {
            List<ITrack> myTrackList = XMLconverter.itemReader(retValRef[1].ArgVal); //Converts xml file to Track
            PlayQueueHandler.AddToPlayQueue(myTrackList[0], index);
        }

        private void RemoveFromPlayQueue(ref List<UPnPArg> retValRef)
        {
			PlayQueueHandler.RemoveFromPlayQueue(Convert.ToInt32(retValRef[1].ArgVal));
        }

        private string GetPos() //returns how far into the track MPlayer is
        {
            return Player.GetPosition();
        }

        private void SetPos(ref List<UPnPArg> retValRef) //used for going back and forth in a track
        {
            Player.SetPosition(Convert.ToInt32(retValRef[1].ArgVal));
			retValRef = null;
        }

        private string GetVol()
        {
            return Player.GetVolume();
        }

        private void SetVol(ref List<UPnPArg> retValRef)
        {
            Player.SetVolume(Convert.ToInt32(retValRef[2].ArgVal));
			Console.WriteLine("\nInside Setvol!");
        }

        private void SubscribeToWrapper()
        {
            Player.EOF_Event += NewSongHandler;
        }


        private void SubscribeToSink()
        {
            UPnPSink.ActionEvent += UPnPHandler;
        }

        private void UPnPHandler(object e, UPnPEventArgs args, CallBack cb)
        {
            string action = args.Action;
			bool CallCB = true;
			Console.WriteLine(">> Inside UPnPHandler: " + action);
            List<UPnPArg> returnVal = args.Args;

            if (action != "Browse")
            {
				//returnVal = null;
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

                case "Prev":
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

 		private List<UPnPArg> CreatePosArgs (List<UPnPArg> inArgs)
		{
			List<UPnPArg> createdArgs = new List<UPnPArg>();
			createdArgs.Add(new UPnPArg("CurrentPosition", GetPos()));

			//createdArgs.Add(new UPnPArg("Duration", PlayQueueHandler.GetCurrentTrack().Duration));
			createdArgs.Add(new UPnPArg("Duration", "100"));

			return createdArgs;
		}

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
