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
		List<ITrack> GetQueue();
    }

    public class PlayqueueHandler : IPlayqueueHandler
    {
        private int _Index;
        private List<ITrack> _Queue; 

		private string _PlayQueueChanged;

		public string PlayQueueChanged
		{
			get{return _PlayQueueChanged;}
			set
			{
				_PlayQueueChanged = value;

				UPnPArg arg = new UPnPArg("PlayQueueChanged", _PlayQueueChanged);

				PropertyChangedEvent.Fire(arg);
			}
		}

        public PlayqueueHandler()
        {
            _Index = 0;
            _Queue = new List<ITrack>();

			/*
			var trk = new Track();
			trk.FileName = "filnavn";
			trk.Album = "albumnavn";
			trk.DeviceIP = "127.0.0.1";
			trk.Artist = "artist";
			trk.Genre = "genre";

			AddToPlayQueue(trk);
			*/
        }

        public ITrack GetNextTrack ()
		{
			if (_Index < _Queue.Count) 
			{
				++_Index;
				return _Queue [_Index-1];
			} 
			else 
			{
				ITrack dummy = new Track();
				dummy.Path = "";
				return dummy;
			}
        }

        public ITrack GetPrevTrack()
        {
            if (_Index > 1) 
			{
				--_Index;
				return _Queue [_Index-1];
			} 
			else 
			{
				ITrack dummy = new Track();
				dummy.Path = "";
				return dummy;
			}
        }

        public ITrack GetTrack (int index)
		{
			if (index <= _Queue.Count && index > 0) 
			{
				PlayQueueChanged = "GET";
				_Index = index;
				return _Queue [_Index-1];
			}
			else 
			{
				Track dummy = new Track();
				dummy.Path = "";
				return dummy;
			}
        }

        public void AddToPlayQueue(ITrack src)
        {
			src.ParentID = "playqueue";
            _Queue.Add(src);
			PlayQueueChanged = "TrackAdded";
        }

        public void AddToPlayQueue(ITrack src, int index)
        {
			src.ParentID = "playqueue";
            if(index <= _Queue.Count)
                _Queue.Insert(index, src);

			PlayQueueChanged = "TrackAdded";
        }

        public void RemoveFromPlayQueue(int index)
        {
            if (index <= _Queue.Count)
            {
                _Queue.RemoveAt(index-1);
                if (index < _Index)
                    --_Index;
            }
			PlayQueueChanged = "TrackRemoved";
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
