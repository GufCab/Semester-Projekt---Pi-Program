using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaybackCtrl
{
    /// <summary>
    /// IPlaylistHandler kan holde styr på en "afspilningskø".
    /// Når man kalder GetNextTrack, bliver det næste nummer returneret.
    /// Hvis man anvener den overloaded version afGetNextTrack(int index), får man et nummer på et bestemt index
    /// 
    /// Det sammme gør sig gældende ved Add metoden
    /// </summary>
    interface IPlaylistHandler
    {
        ITrack GetNextTrack();
        ITrack GetNextTrack(int index);

        void AddToPlayQue();
        void AddToPlayQue(int index);
    }


    public class DummyPlaylistHandler : IPlaylistHandler
    {
        public ITrack GetNextTrack()
        {
            var trk = new Track();
            trk.Name = "Jump.mp3";

            trk.deviceIP = "rtsp://127.0.0.1/";
            trk.path = "";

            return trk;
        }

        public ITrack GetNextTrack(int index)
        {
            var trk = new Track();
            trk.Name = "Jump.mp3";

            trk.deviceIP = "rtsp://127.0.0.1/";
            trk.path = "";

            return trk;
        }

        public void AddToPlayQue()
        {
            
        }

        public void AddToPlayQue(int index)
        {
            
        }
    }


}
