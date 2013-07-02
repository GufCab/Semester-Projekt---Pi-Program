using System;

/// <summary>
/// This namespace implements all features for the MPlayer, including the wrapper which is used by the rest of the program
/// </summary>
namespace MPlayer
{
    /// <summary>
    /// Interface exposing the MediaPlayers functionality.
    /// An implementing class should wrap a MediaPlayer and
    /// feed commands send through this interface to the mediaplayer.
    /// </summary>
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


