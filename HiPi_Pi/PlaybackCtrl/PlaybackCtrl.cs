﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Containers;
using MPlayer;
using UPnP_Device;
using UPnP_Device.UPnPConfig;
using UPnP_Device.XML;
using XMLReader;

namespace PlaybackCtrl
{
    public class PlaybackControl
    {
        private IWrapper Player;
        private IPlayqueueHandler PlayQueueHandler;
        private IUPnP UPnPSink;
        private XMLReader1 XMLconverter;


        public PlaybackControl(IUPnP sink)
        {
            UPnPSink = sink;
            Player = new MPlayerWrapper();
            PlayQueueHandler = new PlayqueueHandler(); 
            XMLconverter = new XMLReader1();
            SubscribeToWrapper();
            SubscribeToSink();
        }

        private void Next()
        {
            var myTrack = PlayQueueHandler.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Prev()
        {
            ITrack myTrack = PlayQueueHandler.GetPrevTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Play()
        {
            var myTrack = PlayQueueHandler.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Pause()
        {
            Player.PauseTrack();
        }

        private void SetCurrentURI(ref List<UPnPArg> retValRef)
        {
            Player.PlayTrack(retValRef[1].ArgVal);
        }

        private void AddToPlayQueue(ref List<UPnPArg> retValRef)
        {
            List<ITrack> myTrackList = XMLconverter.itemReader(retValRef[1].ArgVal); //Converts xml file to Track
            PlayQueueHandler.AddToPlayQueue(myTrackList[0]);
        }


        private void PlayAt(ref List<UPnPArg> retValRef)
        {
            var myTrack = PlayQueueHandler.GetTrack(Convert.ToInt32(retValRef[1].ArgVal));
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

        private double GetPos() //returns how far into the track MPlayer is
        {
            double pos = Convert.ToDouble(Player.GetPosition());
            return pos;
        }

        private void SetPos(ref List<UPnPArg> retValRef) //used for going back and forth in a track
        {
            Player.SetPosition(Convert.ToInt32(retValRef[1].ArgVal));
        }

        private double GetVol()
        {
            double vol = Convert.ToDouble(Player.GetVolume());
            return vol;
        }

        private void SetVol(ref List<UPnPArg> retValRef)
        {
            Player.SetVolume(Convert.ToInt32(retValRef[1].ArgVal));
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
            List<UPnPArg> returnVal = args.Args;

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

                //case "AddAt":
                //    AddToPlayQueue(ref returnVal); //Also needs an int, index. Is it contained in returnVal?
                //    returnVal = null;
                //    break;

                case "PlayAt":
                    PlayAt(ref returnVal);
                    returnVal = null;
                    break;

                case "Remove":
                    RemoveFromPlayQueue(ref returnVal);
                    returnVal = null;
                    break;

                case "SetVol":
                    SetVol(ref returnVal);
                    returnVal = null;
                    break;

                case "SetPos":
                    SetPos(ref returnVal);
                    break;

                case "GetVolume":
                    returnVal.Add(new UPnPArg("GetVol", GetVol().ToString())); //return the volume
                    break;

                case "GetPos":
                    returnVal.Add(new UPnPArg("GetPos", GetPos().ToString())); //return the position
                    break;

                default:
                    Console.WriteLine("PLaybackControl class switchcase default");
                    break;
            }

            Console.WriteLine("PlaybackCtrl ready for callback");
            cb(returnVal, args.Action);
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
