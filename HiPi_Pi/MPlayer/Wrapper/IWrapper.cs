using System;

namespace MPlayer
{
	public interface IWrapper
	{
		void PlayTrack(string path);
		void PauseTrack();
	    void SetPos(int pos);
	    void SetVolume(int pos);

	    string GetVolume();
        string GetPos();
		string GetPaused();
		string GetPlayingFile();

	}
}

