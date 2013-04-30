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
    public interface IPlaylistHandler
    {
        ITrack GetNextTrack();
        ITrack GetPrevTrack();
        ITrack GetTrack(int index);
        
        void AddToPlayQue(ITrack src);
        void AddToPlayQue(string src, int index);
    }


    public class DummyPlaylistHandler : IPlaylistHandler
    {
        private ITrack CurTrk;


        public ITrack GetNextTrack()
        {
            if (CurTrk != null)
            {
                return CurTrk;
            }

            //else:
            ITrack trk = new Track();
            trk.Protocol = "rtsp://";
            trk.DeviceIP = "127.0.0.1/";
            trk.Path = "";
            trk.FileName = "Jump.mp3";
            
            trk.Title = "Jump";

            return trk;
        }


        public ITrack GetTrack(int index)
        {
            var trk = new Track();
            trk.Protocol = "rtsp://";
            trk.DeviceIP = "127.0.0.1/";
            trk.Path = "";
            trk.FileName = "Jump.mp3";

            return trk;
        }

        public ITrack GetPrevTrack()
        {
            var trk = new Track();
            trk.Protocol = "rtsp://";
            trk.DeviceIP = "127.0.0.1/";
            trk.Path = "";
            trk.FileName = "Jump.mp3";

            return trk;
        }


        public void AddToPlayQue(ITrack path)
        {
            //Add the track at path to the bottom of the playqueue
            CurTrk = path;
        }

        public void AddToPlayQue(string path, int index)
        {
            //Add the track at path to position index of the playqueue
        }
    }


}
