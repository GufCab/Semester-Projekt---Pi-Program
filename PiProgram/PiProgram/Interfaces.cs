using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PiProgram
{
    public interface IWrapper
    {
        StreamReader GetOutputStream();
        void GetPos();
        void GetTimeLeft();
        void PauseTrack();
        void PlayTrack(string path);
        void SetPos(int pos);
        void SetVolumeAbs(int vol);
        void SetVolumeRel(int vol);
        void StartMplayer();
    }
}
