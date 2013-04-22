using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPlayer;

namespace PlaybackCtrl
{
    public class PlaybackCtrl
    {
        private IWrapper MPlayer;
        private IPlaylistHandler Playlist;

        //Constructor:
        public PlaybackCtrl()
        {
            MPlayer = new Wrapper();
            Playlist = new DummyPlaylistHandler(/*Path to DB*/); // skal obviously ikke hedde Dummy når den er færdig
        }
        
        public void Next()
        {
            var myTrack = Playlist.GetNextTrack();
            MPlayer.PlayTrack(myTrack.Path);
        }

        public void Prev()
        {
            var myTrack = Playlist.GetPrevTrack();
            MPlayer.PlayTrack(myTrack.Path);
        }

        public void Pause()
        {
            MPlayer.PauseTrack();
        }

        public string GetNextTrack()
        {
            //todo: Look in database. What is the next track to be played?
        }

        public void AddURI(string path)
        {
            Playlist.AddToPlayQue(path);
        }
    }
}
