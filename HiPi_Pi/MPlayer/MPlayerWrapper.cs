using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiProgram
{
    public class Wrapper : IWrapper
    {
        public static readonly TimeSpan MaxWait = TimeSpan.FromMilliseconds(3000);

        private WrapperMPlayerCtrl _mPlayerCtrl;
        private AutoResetEvent _mReceived;

        private string retVal;

        public Wrapper()
        {
            _mPlayerCtrl = new WrapperMPlayerCtrl();
        }

        public void PlayTrack(string path)
        {
            _mPlayerCtrl.PlayTrack(path);
        }

        public string GetPos()
        {
            try
            {
                _mReceived = new AutoResetEvent(false);
                _mPlayerCtrl.timePosFired += TimePosHandler;
                _mPlayerCtrl.GetPos();

                //Wait MaxWait seconds for event to be fired
                //if not, throw exception.
                _mReceived.WaitOne(MaxWait);
            }
            catch (Exception)
            {
                //ToDo Do stuff with exception..
            }
            
            return retVal;
        }

        private void TimePosHandler(object e, RetValEventData args)
        {
            //Do stuff with e and args
            retVal = args.Data;

            //ResetEvent
            _mReceived.Set();
        }


        public string GetTimeLeft()
        {
            return "";
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