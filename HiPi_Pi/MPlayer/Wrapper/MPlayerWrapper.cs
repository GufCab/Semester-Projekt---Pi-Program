using System;
using System.IO;
using System.Diagnostics;
using MPlayer;

namespace MPlayer
{
    public class MPlayerWrapper : IWrapper
    {
        private MPlayerOut mout;
        private StreamReader outStream;
        private StreamWriter inStream;
        private Process mplayer;

        public string GetVolume()
        {
            return "";
        }

        public void SetVolume(int vol)
        {
            
        }

        public void SetPosition(int pos)
        {
            
        }

        public MPlayerWrapper()
        {
            SetupMPlayer();

            mout = new MPlayerOut(outStream);
            Subscribe();
        }

        private void SetupMPlayer()
        {
            mplayer = new Process();
            var startInfo = new ProcessStartInfo();

            string arguments = "-idle -quiet -slave ";

            startInfo.FileName = "mplayer";
            startInfo.Arguments = arguments;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            startInfo.UseShellExecute = false;

            mplayer.StartInfo = startInfo;

            //begin mplayer in idle
            mplayer.Start();

            outStream = mplayer.StandardOutput;
            inStream = mplayer.StandardInput;
        }

        private void Subscribe()
        {
            mout.outPut += new MPlayerOut.OutHandle(OutputEventHandler);
        }

        private void OutputEventHandler(object e, InputData args)
        {
            Console.WriteLine(args.Data);
        }

        public void PlayTrack(string path)
        {
            string order = "loadfile " + path;
            inStream.WriteLine(order);
        }

        public void PauseTrack()
        {
            inStream.WriteLine("pause");
        }

        public string GetPosition()
        {
            inStream.WriteLine("get_time_pos");

            return "";
        }

        public string GetPaused()
        {
            return "";
        }

        public string GetPlayingFile()
        {
            return "";
        }
    }
}

