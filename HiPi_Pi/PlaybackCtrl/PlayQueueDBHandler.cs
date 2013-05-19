﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaybackCtrl
{
    public interface IPlayQueueDBHandler
    {
        //Skal returnere MetaData - Konverteres til ITrack efterfølgende
        ITrack GetTrack(int index);
        void AddToPlayQueue(string s);
        void AddToPlayQueue(string s, int index);
        int GetNumberOfTracks();
        void RemoveFromPlayQueue(int index);
    }

    public class DBHandlerStub
    {
        private int _numberOfTracks = 0;

        //Returns metadata from track at position 'index' in playqueue
        public ITrack GetTrack(int index)
        {
            throw new NotImplementedException();
        }

        //Adds track 's' to the bottom of the playqueue
        public void AddToPlayQueue(string s)
        {
            throw new NotImplementedException();
        }

        //Adds track 's' at position 'index' in the playqueue
        public void AddToPlayQueue(string s, int index)
        {
            throw new NotImplementedException();
        }

        //Returns number of tracks in playqueue
        public int GetNumberOfTracks()
        {
            throw new NotImplementedException();
        }

        //Removes track identified by 's' from playqueue
        public void RemoveFromPlayQueue(int index)
        {
            throw new NotImplementedException();
        }
    }


    class PlayQueueDBHandler : IPlayQueueDBHandler
    {
        private int _numberOfTracks = 0;

        //Returns metadata from track at position 'index' in playqueue
        public ITrack GetTrack(int index)
        {
            throw new NotImplementedException();
        }

        //Adds track 's' to the bottom of the playqueue
        public void AddToPlayQueue(string s)
        {
            throw new NotImplementedException();
        }

        //Adds track 's' at position 'index' in the playqueue
        public void AddToPlayQueue(string s, int index)
        {
            throw new NotImplementedException();
        }

        //Returns number of tracks in playqueue
        public int GetNumberOfTracks()
        {
            throw new NotImplementedException();
        }

        //Removes track identified by 's' from playqueue
        public void RemoveFromPlayQueue(int index)
        {
            throw new NotImplementedException();
        }
    }
}