using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Playback
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
        
        ITrack GetPrevTrack();

        void AddToPlayQue(string src);
        void AddToPlayQue(string src, int index);
    }


    public class DummyPlaylistHandler : IPlaylistHandler
    {
        public ITrack GetNextTrack()
        {
            ITrack trk = new Track();
            trk.Name = "Jump.mp3";

            trk.DeviceIP = "rtsp://127.0.0.1/";
            trk.Path = "";

            return trk;
        }

        public ITrack GetNextTrack(int index)
        {
            var trk = new Track();
            trk.Name = "Jump.mp3";

            trk.DeviceIP = "rtsp://127.0.0.1/";
            trk.Path = "";

            return trk;
        }

        public ITrack GetPrevTrack()
        {
            var trk = new Track();
            trk.Name = "Jump.mp3";

            trk.DeviceIP = "rtsp://127.0.0.1/";
            trk.Path = "";

            return trk;
        }


        public void AddToPlayQue(string path)
        {
            //Add the track at path to the bottom of the playqueue
        }

        public void AddToPlayQue(string path, int index)
        {
            //Add the track at path to position index of the playqueue
        }
    }


}
