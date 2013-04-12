using System;

namespace PiProgram
{
    public interface IWrapper
    {
        int GetPos();
        int GetTimeLeft();
        void PauseTrack();
        void PlayTrack(string path);
        void SetPos(int pos);
        void SetVolume(int vol);
    }
}