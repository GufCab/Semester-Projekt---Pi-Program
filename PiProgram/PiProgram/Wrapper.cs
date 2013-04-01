using System;
using System.Diagnostics;
using System.IO;
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
        
        public Wrapper(string path)
        {
            var mplayer = new Process();
            var startInfo = new ProcessStartInfo();

            string arguments = "";
            arguments = "-slave" + " " + path;
            
            startInfo.FileName = "mplayer";
            startInfo.Arguments = arguments;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            mplayer.StartInfo = startInfo;
            mplayer.Start();

            _inStream = mplayer.StandardInput;
            _outStream = mplayer.StandardOutput;

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
