using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaybackCtrl
{
    public interface IPlayQueueDB
    {
        ITrack GetTrack(int index);
        void AddToPlayQueue();
        void AddToPlayQueue(int index);
    }

    class PlayQueueDB : IPlayQueueDB
    {
        //Returnerer MetaData fra tracket på plads "index" i PlayQueuen
        public ITrack GetTrack(int index)
        {
            throw new NotImplementedException();
        }

        //Tilføjer til bunden af PlayeQueue
        public void AddToPlayQueue()
        {
            throw new NotImplementedException();
        }

        //Tilføjer på plads "index" i PlayQueue
        public void AddToPlayQueue(int index)
        {
            throw new NotImplementedException();
        }
    }
}
