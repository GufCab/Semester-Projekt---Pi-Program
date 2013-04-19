using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPlayer;

namespace Playback
{
    public class PlaybackCtrl
    {
        private IWrapper MPlayer;
        

        //Constructor:
        public PlaybackCtrl()
        {
            MPlayer = new Wrapper();


        }
        
        public void Play()
        {
            MPlayer.PlayTrack();
            
        }

        public string getNextTrack()
        {
            //todo: Look in database. What is the next track to be played?

        }

        public void AddURI(string path)
        {
            
        }
    }
}
