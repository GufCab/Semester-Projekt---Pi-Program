using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPlayer;

namespace PlaybackCtrl
{
    public class PlaybackControl
    {
        private IWrapper Player;
        private IPlaylistHandler Playlist;
        private IUPnPSink UPnPSink;

        public PlaybackControl(IUPnPSink sink)
        {
            UPnPSink = sink;
            Player = new MPlayerWrapper();
            Playlist = new PlaylistHandler(); //Forbindelse til DB laves i DBInterface.cs
            SubscribeToWrapper();
            SubscribeToSink(); //Not implemented
        }

        private void Next()
        {
            var myTrack = Playlist.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Prev()
        {
            ITrack myTrack = Playlist.GetPrevTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void Play()
        {
            var myTrack = Playlist.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        private void PlayAt(int index)
        {
            var myTrack = Playlist.GetTrack(index);
            Player.PlayTrack(myTrack.Path);
        }

        private void Pause()
        {
            Player.PauseTrack();
        }


        private void AddToPlayQueue(string path)
        {
            Playlist.AddToPlayQue(path);
        }

        private void AddToPlayQueue(string path, int index)
        {
            Playlist.AddToPlayQue(path, index);
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
            UPnPSink.UPnP_Events += UPnPHandler;
            //When UPnPInvoke happens, call UPnPHandler
        }


        private void UPnPHandler(object e, UPnPEventArgs args, Callback cb)
        {
            //Switch-case / Gufs magic dictionary (se TCPResponses i TCP i UPnP_Device)
            //Call function (Functions called this way can be made private)
            string action = args.action;

            switch (action)
            {
                case "Play":
                    Play();
                    break;

                case "Next":
                    Next();
                    break;

                case "Prev":
                    Prev();
                    break;

                case "Pause":
                    Pause();
                    break;

                case "Add":
                    AddToPlayQueue(args.somesString);
                    break;

                case "AddAt":
                    AddToPlayQueue(args.someString, args.position);
                    break;

                case "SetVol":
                    SetVol(args.desiredVol);
                    break;

                case "SetPos":
                    SetPos(args.desiredPos);
                    break;

                case "GetVol":
                    GetVol(); //return the volume
                    break;

                case "GetPos":
                    GetPos(); //return the position
                    break;
            }
            cb(someParameters);
        }

        private void NewSongHandler (object e, EventArgs args)
        {
            //Afspiller næste sang. Hvis playqueue er tom afsluttes afspilning
            if (Playlist.GetNumberOfTracks() > Playlist.GetCurrentTrackIndex())
            {
                Next();
            }
        }
    }
}
