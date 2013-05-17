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

        //Constructor:
        public PlaybackControl(IUPnPSink sink)
        {
            UPnPSink = sink;
            Player = new MPlayerWrapper();
            Playlist = new PlaylistHandler(); //Forbindelse til DB laves i DBInterface.cs
            SubscribeToWrapper();
            SubscribeToSink(); //Not implemented
        }
        
        public void Next()
        {
            var myTrack = Playlist.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        public void Prev()
        {
            ITrack myTrack = Playlist.GetPrevTrack();
            Player.PlayTrack(myTrack.Path);
        }

        public void Play()
        {
            var myTrack = Playlist.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        public void PlayAt(int index)
        {
            var myTrack = Playlist.GetTrack(index);
            Player.PlayTrack(myTrack.Path);
        }

        public void Pause()
        {
            Player.PauseTrack();
        }


        public void AddToPlayQueue(string path)
        {
            Playlist.AddToPlayQue(path);
        }

        public void AddToPlayQueue(string path, int index)
        {
            Playlist.AddToPlayQue(path, index);
        }

        public double GetPos() //returns how far into the track MPlayer is
        {
            double pos = Convert.ToDouble(Player.GetPosition());
            return pos;
        }

        public void SetPos(int pos) //used for going back and forth in a track
        {
            Player.SetPosition(pos);
        }

        public double GetVol()
        {
            double vol = Convert.ToDouble(Player.GetVolume());
            return vol;
        }

        public void SetVol(int vol)
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


        private void UPnPHandler()
        {
            //Switch-case / Gufs magic dictionary (se TCPResponses i TCP i UPnP_Device)
            //Call function (Functions called this way can be made private)


            //Raise callback event
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
