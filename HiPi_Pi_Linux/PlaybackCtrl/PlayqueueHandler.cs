using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Containers;
using System.Threading;

namespace PlaybackCtrl
{
	/// <summary>
	/// This interface contains the needed features to implement a playqueue
	/// </summary>
    public interface IPlayqueueHandler
    {
        ITrack GetNextTrack();
        ITrack GetPrevTrack();
        ITrack GetTrack(int index);
        
        void AddToPlayQueue(ITrack trk);
        void AddToPlayQueue(ITrack trk, int index);
        void RemoveFromPlayQueue(int index);

        int GetNumberOfTracks();
        int GetCurrentTrackIndex();
        ITrack GetCurrentTrack();
		List<ITrack> GetQueue();

		string PlayQueueChanged { get; set; }
    }

    /// <summary>
    /// This class handles the playqueue for the system
    /// </summary>
    public class PlayqueueHandler : IPlayqueueHandler
    {
        //Used for keeping track of the current position in the playqueue
        private int _Index;
		private Mutex mu;

        //List containing the Tracks in the playqueue
        private List<ITrack> _Queue; 

		private string _PlayQueueChanged;

        /// <summary>
        /// Evented UPnP variable.
        /// Everytime playqueue changes, PropertyChangedEvent is raised
        /// and the control point is notified.
        /// </summary>
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

        /// <summary>
        /// Constructor. Creates the List in which the Tracks are stored and sets the _index to 0.
        /// </summary>
        public PlayqueueHandler()
        {
            _Index = 0;
            _Queue = new List<ITrack>();
			mu = new Mutex();
        }

        /// <summary>
        /// Returns the next Track in the playqueue unless _Index is at the last position in the playqueue. 
        /// If _Index is at the last position in the playqueue, returns a dummy Track.
        /// </summary>
        /// <returns>Next Track in playqueue or dummy Track</returns>
        public ITrack GetNextTrack ()
		{
			mu.WaitOne();
			if (_Index < _Queue.Count) 
			{
				++_Index;
				mu.ReleaseMutex();
				return  _Queue [_Index-1];
			} 
			else 
			{
				ITrack dummy = new Track();
				dummy.Path = "";
				mu.ReleaseMutex();
				return dummy;
			}
        }

        /// <summary>
        /// Returns the previous Track in the playqueue unless _Index is at the first position in the playqueue.
        /// If _Index is at the first position in the playqueue (or 0), returns a dummy Track.
        /// </summary>
        /// <returns>Previous Track in playqueue or dummy Track</returns>
        public ITrack GetPrevTrack()
        {
			mu.WaitOne();
            if (_Index > 1) 
			{
				--_Index;
				mu.ReleaseMutex();
				return _Queue [_Index-1];
			} 
			else 
			{
				ITrack dummy = new Track();
				dummy.Path = "";
				mu.ReleaseMutex();
				return dummy;
			}
        }

        /// <summary>
        /// Returns the Track at specific position in the playqueue. If index does not corrospond to a position in the playqueue function returns a dummy Track.
        /// </summary>
        /// <param name="index">Integer indicating the position from which to return the Track</param>
        /// <returns>Track at position index in playqueue or dummy Track</returns>
        public ITrack GetTrack (int index)
		{
			mu.WaitOne();
			if (index <= _Queue.Count && index > 0) 
			{
				_Index = index;
				mu.ReleaseMutex();
				return _Queue [_Index-1];
			}
			else 
			{
				Track dummy = new Track();
				dummy.Path = "";
				mu.ReleaseMutex();
				return dummy;
			}

        }

        /// <summary>
        /// Adds Track to bottom of the playqueue.
        /// </summary>
        /// <param name="trk">Track to be added to playqueue</param>
        public void AddToPlayQueue(ITrack trk)
        {
			mu.WaitOne();
			trk.ParentID = "playqueue";
            _Queue.Add(trk);
			PlayQueueChanged = "TrackAdded";
			mu.ReleaseMutex();
        }

        /// <summary>
        /// Adds Track to specific position in playqueue.
        /// </summary>
        /// <param name="trk">Track to be added to the playqueue</param>
        /// <param name="index">Integer indicating position in playqueue to add Track to</param>
        public void AddToPlayQueue(ITrack trk, int index)
        {
			mu.WaitOne();
			trk.ParentID = "playqueue";
            if(index <= _Queue.Count)
                _Queue.Insert(index, trk);

			PlayQueueChanged = "TrackAdded";
			mu.ReleaseMutex();
        }

        /// <summary>
        /// Removes Track from specific position in playqueue
        /// </summary>
        /// <param name="index">Integer indicating position in playqueue to remove Track from</param>
        public void RemoveFromPlayQueue(int index)
        {
			mu.WaitOne();
            if (index <= _Queue.Count)
            {
                _Queue.RemoveAt(index-1);
                if (index < _Index)
                    --_Index;
            }
			PlayQueueChanged = "TrackRemoved";
			mu.ReleaseMutex();
        }

        /// <summary>
        /// Returns current position in playqueue.
        /// </summary>
        /// <returns>Integer indicating position</returns>
        public int GetCurrentTrackIndex()
        {
            return _Index;
        }

        /// <summary>
        /// Returns Track at current position in playqueue
        /// </summary>
        /// <returns>Track at position</returns>
        public ITrack GetCurrentTrack()
        {
            return _Queue[_Index];
        }

        /// <summary>
        /// Returns number of Tracks in playqueue
        /// </summary>
        /// <returns>Integer indicating number of Tracks</returns>
        public int GetNumberOfTracks()
        {
            return _Queue.Count;
        }

        /// <summary>
        /// Returns entire playqueue
        /// </summary>
        /// <returns>List of Tracks in playqueue</returns>
		public List<ITrack> GetQueue ()
		{
			return _Queue;
		}
    }
}
