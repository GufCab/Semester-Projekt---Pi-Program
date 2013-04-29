using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPlayer;
using Playback;

namespace Playback
{
    public class PlaybackCtrl
    {
        private IWrapper MPlayer;
        private IPlaylistHandler Playlist;

        //Constructor:
        public PlaybackCtrl()
        {
            MPlayer = new MPlayerWrapper();
            Playlist = new DummyPlaylistHandler(/*Path to DB*/); // skal obviously ikke hedde Dummy når den er færdig
        }
        
        public void Next()
        {
            ITrack myTrack = Playlist.GetNextTrack();
            MPlayer.PlayTrack(myTrack.Path);
        }

        public void Prev()
        {
            ITrack myTrack = Playlist.GetPrevTrack();
            MPlayer.PlayTrack(myTrack.Path);
        }

        public void Play()
        {
            var trk = Playlist.GetNextTrack();
            MPlayer.PlayTrack(trk.Path);
        }

        public void Pause()
        {
            MPlayer.PauseTrack();
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
            double pos = Convert.ToDouble(MPlayer.GetPosition());
            return pos;
        }

        public void SetPos(int pos) //used for going back and forth in a track
        {
            MPlayer.SetPosition(pos);
        }

        public double GetVol()
        {
            double vol = Convert.ToDouble(MPlayer.GetVolume());
            return vol;
        }

        public void SetVol(int vol)
        {
            MPlayer.SetVolume(vol);
        }


        //Where should this be called? in the constructor?
        private void SubscribeToWrapper()
        {
            MPlayer.Subscribe();
        }
    }
}
