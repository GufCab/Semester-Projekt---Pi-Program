using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaybackCtrl
{
    /// <summary>
    /// IPlayqueueHandler kan holde styr på en "afspilningskø".
    /// </summary>
    public interface IPlayqueueHandler
    {
        ITrack GetNextTrack();
        ITrack GetPrevTrack();
        ITrack GetTrack(int index);
        
        void AddToPlayQueue(ITrack src);
        void AddToPlayQueue(ITrack src, int index);
        void RemoveFromPlayQueue(int index);

        //int GetNumberOfTracks();
        int GetCurrentTrackIndex();
        ITrack GetCurrentTrack();
    }

    public class PlayqueueHandler : IPlayqueueHandler
    {
        //private ITrack _CurrentTrack;
        private int _Index;
        private List<ITrack> _Queue; 

        public PlayqueueHandler()
        {
            _Index = 0;
            _Queue = new List<ITrack>();
        }

        public ITrack GetNextTrack()
        {
            ++_Index;
            return _Queue[_Index];
        }

        public ITrack GetPrevTrack()
        {
            --_Index;
            return _Queue[_Index];
        }

        public ITrack GetTrack(int index)
        {
            _Index = index;
            return _Queue[_Index];
        }

        public void AddToPlayQueue(ITrack src)
        {
            _Queue.Add(src);
        }

        public void AddToPlayQueue(ITrack src, int index)
        {
            if(index <= _Queue.Count)
                _Queue.Insert(index, src);
        }

        public void RemoveFromPlayQueue(int index)
        {
            if (index <= _Queue.Count)
            {
                _Queue.RemoveAt(index);
                if (index < _Index)
                    --_Index;
            }
                
        }

        public int GetCurrentTrackIndex()
        {
            return _Index;
        }

        public ITrack GetCurrentTrack()
        {
            return _Queue[_Index];
        }
    }

    /*
    public class DummyPlayqueueHandler : IPlayqueueHandler
    {
        private IPlayQueueDBHandler _playQ;
        private int _currentTrackIndex = 0;
        private ITrack _currentTrack;

        public DummyPlayqueueHandler()
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

        public void AddToPlayQueue(string s)
        {
            _playQ.AddToPlayQueue(s);
        }

        public void AddToPlayQueue(string s, int index)
        {
            _playQ.AddToPlayQueue(s, index);
        }

        public void RemoveFromPlayQueue(int index)
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
     * */
}
