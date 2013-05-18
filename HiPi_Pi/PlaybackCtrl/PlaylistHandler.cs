using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaybackCtrl
{
    /// <summary>
    /// IPlaylistHandler kan holde styr på en "afspilningskø".
    /// </summary>
    public interface IPlaylistHandler
    {
        ITrack GetNextTrack();
        ITrack GetPrevTrack();
        ITrack GetTrack(int index);
        
        void AddToPlayQue(string src);
        void AddToPlayQue(string src, int index);
        void RemoveFromPlayQue(int index);

        int GetNumberOfTracks();
        int GetCurrentTrackIndex();
        ITrack GetCurrentTrack();
    }

    public class PlaylistHandler : IPlaylistHandler
    {
        private IPlayQueueDBHandler _playQ;
        private int _currentTrackIndex = 0;
        private ITrack _currentTrack;

        public PlaylistHandler()
        {
            _playQ = new PlayQueueDBHandler();
            _currentTrack = new Track();
        }

        public ITrack GetNextTrack()
        {
            _currentTrackIndex++;
            _currentTrack = _playQ.GetTrack(_currentTrackIndex); //Needs to convert from Metadata to Track
            return _currentTrack;
        }

        public ITrack GetPrevTrack()
        {
            if (_currentTrackIndex > 0)
            {
                _currentTrackIndex--;
            }
            _currentTrack = _playQ.GetTrack(_currentTrackIndex);  //Needs to convert from Metadata to Track
            return _currentTrack;
        }

        public ITrack GetTrack(int index)
        {
            _currentTrackIndex = index;
            _currentTrack = _playQ.GetTrack(_currentTrackIndex);  //Needs to convert from Metadata to Track
            return _currentTrack;
        }

        public void AddToPlayQue(string s)
        {
            _playQ.AddToPlayQueue(s);
        }

        public void AddToPlayQue(string s, int index)
        {
            _playQ.AddToPlayQueue(s, index);
        }

        public void RemoveFromPlayQue(int index)
        {
            _playQ.RemoveFromPlayQueue(index);
        }

        public int GetNumberOfTracks()
        {
            return _playQ.GetNumberOfTracks();
        }

        public int GetCurrentTrackIndex()
        {
            return _currentTrackIndex;
        }

        public ITrack GetCurrentTrack()
        {
            return _currentTrack;
        }
    }
}
