using System;

namespace MPlayer
{
    public interface IWrapper
    {
        string GetPos();
        string GetTimeLeft();
        void PauseTrack();
        void PlayTrack(string path);
        void SetPos(int pos);
        void SetVolume(int vol);
    }
}