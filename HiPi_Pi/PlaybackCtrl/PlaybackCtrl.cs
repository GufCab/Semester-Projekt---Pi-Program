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

        private List<string> NextFiveTracks; 

        //Constructor:
        public PlaybackCtrl()
        {
            MPlayer = new Wrapper();
            NextFiveTracks = new List<string>();

        }
        
        public void Play()
        {
            if (NextFiveTracks.Count > 0)
            {
                MPlayer.PlayTrack(NextFiveTracks[0]);
            }
        }

        public void AddURI(string path)
        {
            NextFiveTracks.Add(path);
        }
    }
}
