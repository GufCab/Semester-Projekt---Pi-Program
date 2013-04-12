using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiProgram
{
    public class Wrapper : IWrapper
    {
        private WrapperMPlayerCtrl _mPlayerCtrl;

        public Wrapper()
        {
            _mPlayerCtrl = new WrapperMPlayerCtrl();
        }

        public void PlayTrack(string path)
        {
            _mPlayerCtrl.PlayTrack(path);
        }
        
        public int GetPos()
        {
            return 0;
        }

        public int GetTimeLeft()
        {
            return 0;
        }

        public void PauseTrack()
        {
            _mPlayerCtrl.PauseMPlayer();
        }

        public void SetPos(int pos)
        {
            
        }

        public void SetVolume(int vol)
        {
            
        }
    }
}
