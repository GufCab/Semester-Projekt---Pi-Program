using System;
using System.Diagnostics;
using System.IO;


namespace PiProgram
{
    public class WrapperMPlayerCtrl
    {
        private StreamWriter _inStream;
        private StreamReader _outStream;
        private Process _mplayer;
        private WrapperMOut _mOut;

        public event TimePosFire timePosFired;

        public delegate void TimePosFire(object e, RetValEventData args);

        public event PercPosFire percPosFired;

        public delegate void PercPosFire(object e, RetValEventData args);

        public WrapperMPlayerCtrl()
        {
            _mOut = new WrapperMOut();
        }

        public void PlayTrack(string path)
        {
            SetupProcess(path);
            _mOut.SetOutStream(_outStream);
            _mplayer.Start();
        }

        private void SetupProcess(string path)
        {
            _mplayer = new Process();
            var startInfo = new ProcessStartInfo();

            string arguments = "-slave " + path;

            startInfo.FileName = "mplayer";
            startInfo.Arguments = arguments;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            _mplayer.StartInfo = startInfo;

            _inStream = _mplayer.StandardInput;
            _outStream = _mplayer.StandardOutput;
        }

        public void KillMPlayer()
        {
            if (!_mplayer.HasExited)
                _mplayer.Kill();
        }

        public void PauseMPlayer()
        {
            if (!_mplayer.HasExited)
            {
                try
                {
                    _inStream.WriteLine("pause");
                }
                catch (Exception)
                {
                    //Do error stuff..
                }
            }
        }

        public StreamReader GetOStream()
        {
            return _outStream;
        }

        private void Subscribe()
        {
            _mOut.outPut += new WrapperMOut.OutHandle(DetermineOrder);
        }

        private void DetermineOrder(object e, WrapperMOut.InputData args)
        {
            var str = ReadOutputClass.GetData(args.Data);

            FireEvents(str);
        }

        private void FireEvents(string[] data)
        {
            var evtData = new RetValEventData();
            evtData.Data = data[1];

            //Which events to fire..
            if (data[0] == "ANS_TIME_POSITION")
            {
                //Fire timePosEvent.
                timePosFired(this, evtData);
            }

            if (data[0] == "ANS_PERCENT_POSITION")
            {
                //Fire PercentEvent.
                percPosFired(this, evtData);
            }
        }
    }

    public class RetValEventData : EventArgs
    {
        public string Data { get; set; }
    }
}