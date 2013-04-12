using System;

namespace PiProgram
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