using System;

namespace MPlayer
{
	public interface IWrapper
	{
		void PlayTrack(string path);
		void PauseTrack();
	    void SetPosition(int pos);
	    void SetVolume(int pos);

	    string GetVolume();
        string GetPosition();
		string GetPaused();
		string GetPlayingFile();

	}
}

