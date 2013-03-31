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

            
        }
        
        public StreamReader GetOutputStream()
        {
            
        }

        public int GetPos()
        {
            
        }

        public int GetTimeLeft()
        {
            
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
