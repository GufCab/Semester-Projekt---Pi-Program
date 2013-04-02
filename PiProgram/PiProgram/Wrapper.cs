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
        
        public Wrapper(string path)
        {
            _mplayer = new Process();
            var startInfo = new ProcessStartInfo();

            string arguments = "";
            arguments = "-slave" + " " + path;
            
            startInfo.FileName = "mplayer";
            startInfo.Arguments = arguments;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            _mplayer.StartInfo = startInfo;
            //_mplayer.Start();

            _inStream = _mplayer.StandardInput;
            _outStream = _mplayer.StandardOutput;

        }

        public void StartMplayerThread()
        {
            _playerThread = new Thread(ThreadFunc);
            _playerThread.Start();

            
        }

        private void ThreadFunc()
        {
            while (true)
            {
                _mplayer.Start();
            }
        }
        
        public StreamReader GetOutputStream()
        {
            return null;
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
            
        }

        public void PlaySong(string path)
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
