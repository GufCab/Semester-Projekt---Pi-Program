using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Containers;

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

        int GetNumberOfTracks();
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

        public int GetNumberOfTracks()
        {
            return _Queue.Count;
        }

		public List<ITrack> GetQueue ()
		{
			return _Queue;
		}
    }
}
