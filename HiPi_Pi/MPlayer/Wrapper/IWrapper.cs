using System;

namespace MPlayer
{
    public interface IWrapper
    {
        void PlayTrack(string path);
        void PauseTrack();
        void SetVolume(int vol);
        void SetPosition(int pos);

        string GetPosition();
        string GetVolume();
        string GetPaused(); //Todo Tror bare GetPaused skal væk..
        string GetPlayingFile();

    }
}


