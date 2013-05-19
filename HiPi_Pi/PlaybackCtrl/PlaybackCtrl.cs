using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPlayer;
using UPnP_Device;
using UPnP_Device.UPnPConfig;
using XMLHandler;

namespace PlaybackCtrl
{
    public class PlaybackControl
    {
        private IWrapper Player;
        private IPlayqueueHandler PlayQueueHandler;
        private IUPnP UPnPSink;
        private XMLReader XMLconverter;


        public PlaybackControl(IUPnP sink)
        {
            UPnPSink = sink;
            Player = new MPlayerWrapper();
            PlayQueueHandler = new PlayqueueHandler(); //Forbindelse til DB laves i DBInterface.cs
            XMLconverter = new XMLReader();
            SubscribeToWrapper();
            SubscribeToSink();
        }

        private void Next(ref List<UPnPArg> retValRef)
        {
            var myTrack = PlayQueueHandler.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Prev(ref List<UPnPArg> retValRef)
        {
            ITrack myTrack = PlayQueueHandler.GetPrevTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Play(ref List<UPnPArg> retValRef)
        {
            var myTrack = PlayQueueHandler.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Pause(ref List<UPnPArg> retValRef)
        {
            Player.PauseTrack();
        }

        private void SetCurrentURI(ref List<UPnPArg> retValRef)
        {
            Player.PlayTrack(retValRef[1].ArgVal);
        }

        private void AddToPlayQueue(ref List<UPnPArg> retValRef)
        {
            ITrack myTrack = XMLconverter.Convert(retValRef[1]); //Converts xml file to Track
            PlayQueueHandler.AddToPlayQueue(myTrack);
        }


        private void PlayAt(int index)
        {
            var myTrack = PlayQueueHandler.GetTrack(index);
            Player.PlayTrack(myTrack.Path);
        }
        
        
        private void AddToPlayQueue(string path, int index)
        {
            PlayQueueHandler.AddToPlayQueue(path, index);
        }

        private void RemoveFromPlayQueue(int index)
        {
            PlayQueueHandler.RemoveFromPlayQueue(index);
        }

        private double GetPos() //returns how far into the track MPlayer is
        {
            double pos = Convert.ToDouble(Player.GetPosition());
            return pos;
        }

        private void SetPos(int pos) //used for going back and forth in a track
        {
            Player.SetPosition(pos);
        }

        private double GetVol()
        {
            double vol = Convert.ToDouble(Player.GetVolume());
            return vol;
        }

        private void SetVol(int vol)
        {
            Player.SetVolume(vol);
        }

        private void SubscribeToWrapper()
        {
            Player.EOF_Event += NewSongHandler;
        }

        private void SubscribeToSink()
        {
            UPnPSink.ActionEvent += UPnPHandler;
            //When UPnPInvoke happens, call UPnPHandler
        }


        private void UPnPHandler(object e, UPnPEventArgs args, CallBack cb)
        {
            string action = args.Action;
            List<UPnPArg> returnVal = args.Args;

            switch (args.Action)
            {
                case "Play":
                    Play(ref returnVal);
                    returnVal = null;
                    break;

                case "Next":
                    Next(ref returnVal);
                    returnVal = null;
                    break;

                case "Prev":
                    Prev(ref returnVal);
                    break;

                case "Pause":
                    Pause(ref returnVal);
                    break;

                case "SetNextAVTransportURI":
                    AddToPlayQueue(ref returnVal);
                    break;

                case "SetAVTransportURI":
                    SetCurrentURI(ref returnVal);
                    break;
                    /*
                case "AddAt":
                    AddToPlayQueue(args.someString, args.someInt);
                    break;

                case "Remove":
                    RemoveFromPlayQueue(args.someInt);

                case "SetVol":
                    SetVol(args.desiredVol);
                    break;

                case "SetPos":
                    SetPos(args.desiredPos);
                    break;

                case "GetVolume":
                    returnVal.Add(new UPnPArg("GetVol", GetVol().ToString())); //return the volume
                    break;

                case "GetPos":
                    GetPos(); //return the position
                    break;
                     * */
                default:
                    Console.WriteLine("PLaybackControl class switchcase default");
                    break;
            }

            Console.WriteLine("PlaybackCtrl ready for callback");
            cb(returnVal, args.Action);
        }

        private void NewSongHandler (object e, EventArgs args)
        {
            //Afspiller næste sang. Hvis playqueue er tom afsluttes afspilning
            if (PlayQueueHandler.GetNumberOfTracks() > PlayQueueHandler.GetCurrentTrackIndex())
            {
                //Next();
            }
        }
    }
}
