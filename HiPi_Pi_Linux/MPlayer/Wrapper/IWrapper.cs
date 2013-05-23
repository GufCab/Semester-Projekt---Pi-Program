using System;

namespace MPlayer
{
    public interface IWrapper
    {
        void PlayTrack(string path);
        void PauseTrack();
        void SetVolume(int vol);
        void SetPosition(int pos);

        event MPlayerWrapper.EOFHandle EOF_Event;

        string GetPosition();
        string GetVolume();
        string GetPlayingFile();
		bool GetPaused();
    }
}


