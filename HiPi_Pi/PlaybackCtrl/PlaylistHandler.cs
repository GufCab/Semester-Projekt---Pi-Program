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
    public interface IPlaylistHandler
    {
        ITrack GetNextTrack();
        ITrack GetPrevTrack();
        ITrack GetTrack(int index);
        
        void AddToPlayQue(string src);
        void AddToPlayQue(string src, int index);
    }

    public class PlaylistHandler : IPlaylistHandler
    {
        private IPlayQueueDB _playQ;
        private int _currentTrack = 0;

        public PlaylistHandler()
        {
            _playQ = new PlayQueueDB();
        }

        public ITrack GetNextTrack()
        {
            _currentTrack++;
            ITrack trk = new Track();
            _playQ.GetTrack(_currentTrack);
            //Convert MetaData to Track
            return trk;
        }
        
        public ITrack GetPrevTrack()
        {
            if (_currentTrack > 0)
            {
                _currentTrack--;
            }
            ITrack trk = new Track();
            _playQ.GetTrack(_currentTrack);
            //Convert MetaData to Track
            return trk;
        }

        public ITrack GetTrack(int index)
        {
            _currentTrack = index;
            ITrack trk = new Track();
            _playQ.GetTrack(_currentTrack);
            //Convert MetaData to Track
            return trk;
        }

        public void AddToPlayQue(string s)
        {
            _playQ.AddToPlayQueue(s);
        }

        public void AddToPlayQue(string s, int index)
        {
            _playQ.AddToPlayQueue(s,index);
        }
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


        public void AddToPlayQue(string path)
        {
            if (CurTrk == null)
            {
                CurTrk = new Track();
            }
            //Add the track at path to the bottom of the playqueue
            CurTrk.Path = path;
        }

        public void AddToPlayQue(string path, int index)
        {
            //Add the track at path to position index of the playqueue
        }
    }


}
