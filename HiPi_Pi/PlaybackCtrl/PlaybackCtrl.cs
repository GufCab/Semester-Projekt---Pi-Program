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
        private IWrapper Player;
        private IPlaylistHandler Playlist;

        //Constructor:
        public PlaybackCtrl()
        {
            Player = new MPlayerWrapper();
            Playlist = new DummyPlaylistHandler(/*Path to DB*/); // skal obviously ikke hedde Dummy når den er færdig
        }
        
        public void Next()
        {
            ITrack myTrack = Playlist.GetNextTrack();
            Player.PlayTrack(myTrack.Path);
        }

        public void Prev()
        {
            ITrack myTrack = Playlist.GetPrevTrack();
            Player.PlayTrack(myTrack.Path);
        }

        public void Play()
        {
            var trk = Playlist.GetNextTrack();
            Player.PlayTrack(trk.Path);
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


        //Where should this be called? in the constructor?
        private void SubscribeToWrapper()
        {
            Player.EOF_Event += AiksVeryOwnSpecialHandler;
        }

        private void AiksVeryOwnSpecialHandler(object e, EventArgs args)
        {
            ////Her skal playbackctrl finde ud af hvad der skal ske (bliver kaldt når sangen er slut)
            //if (Playlist.PlayQueueEmpty)
            //{
            //    //stop playing
            //}
            //else
            //{
            //    Next();
            //}
        }
         
    }
}
