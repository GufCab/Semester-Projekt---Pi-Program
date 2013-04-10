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
        
        public WrapperMPlayerCtrl()
        {
            _mOut = new WrapperMOut();
        }

        public void PlayTrack(string path)
        {
            SetupProcess(path);
           // _mOut
            _mplayer.Start();
        }

        private void SetupProcess(string path)
        {
            _mplayer = new Process();
            var startInfo = new ProcessStartInfo();

            string arguments = arguments = "-slave " + path;

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
            if(!_mplayer.HasExited)
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

    }
}
