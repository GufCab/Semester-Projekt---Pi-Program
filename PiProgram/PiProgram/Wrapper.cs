using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiProgram
{
    public class Wrapper : IWrapper
    {
        //Dette er selve de streams der skal skrives og læses fra!
        private StreamWriter _inStream;
        private StreamReader _outStream;
        private Process _mplayer;
        private Thread _playerThread;


        //Skal den have den her, hvor den starter med en Path oder was?
        public Wrapper(string path)
        {
            SetupProcess(path);
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

        public void PlayTrack(string path)
        {
            if (!_mplayer.HasExited)
            {
                var execOrder = "loadfile " + path;
                _inStream.WriteLine(execOrder);
            }
            else
            {
                SetupProcess(path);
                StartMplayer();
            }
        }

        public void StartMplayer()
        {
            _mplayer.Start();
        }

        public StreamReader GetOutputStream()
        {
            return _outStream;
        }

        public void GetPos()
        {
        }

        public void GetTimeLeft()
        {
        }

        public void PauseTrack()
        {
        }
        

        public void SetPos(int pos)
        {
        }

        public void SetVolumeAbs(int vol)
        {
        }

        public void SetVolumeRel(int vol)
        {
        }
    }
}