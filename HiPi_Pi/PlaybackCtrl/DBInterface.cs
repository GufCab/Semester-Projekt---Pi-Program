using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaybackCtrl
{
    public interface IPlayQueueDB
    {
        //Skal returnere MetaData - Konverteres til ITrack efterfølgende
        ITrack GetTrack(int index);
        void AddToPlayQueue(string s);
        void AddToPlayQueue(string s, int index);
        int GetNumberOfTracks();
    }

    class PlayQueueDB : IPlayQueueDB
    {
        private int _numberOfTracks = 0;

        //Returnerer MetaData fra tracket på plads "index" i PlayQueuen
        public ITrack GetTrack(int index)
        {
            throw new NotImplementedException();
        }

        //Tilføjer til bunden af PlayeQueue
        public void AddToPlayQueue(string s)
        {
            throw new NotImplementedException();
        }

        //Tilføjer på plads "index" i PlayQueue
        public void AddToPlayQueue(string s, int index)
        {
            throw new NotImplementedException();
        }

        //Ser om listen er tom
        public int GetNumberOfTracks()
        {
            throw new NotImplementedException();
        }
    }
}
