using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MPlayer;

namespace MPlayer
{
    public class MPlayerWrapper : IWrapper
    {
        private MPlayerOut mout;
        private StreamReader outStream;
        private StreamWriter inStream;
        private Process mplayer;

        private AutoResetEvent timeResetEvent;
        private string timeRetVal;

        private AutoResetEvent volResetEvent;
        private string volRetval;

        private AutoResetEvent fileResetEvent;
        private string fileRetval;

        public delegate void TimePosHandle(object e, InputData args);
        public event TimePosHandle TimePosEvent;

        public delegate void VolGetHandle(object e, InputData args);
        public event VolGetHandle VolGetEvent;

        public delegate void FileGetHandle(object e, InputData args);
        public event FileGetHandle FileGetEvent;

        public delegate void EOFHandle(object e, EventArgs args);
        public event EOFHandle EOF_Event;

        

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
            var dataArray = InterpreterClass.DataToArray(args.Data);
            FireEvents(dataArray);
        }

        private void FireEvents(string[] arr)
        {
            //Console.WriteLine(arr[0]);
            if (arr[0].Contains("EOF code: 1"))
                FireEOF_Event();

            if (arr.Length > 1)
            {
                //This is the value from MPlayer
                var inputData = new InputData(arr[1]);

                switch (arr[0])
                {
                    case "ANS_TIME_POSITION":
                        FirePosEvent(inputData);
                        break;
                    case "ANS_volume":
                        FireVolEvent(inputData);
                        break;
                    case "ANS_path":
                        FireFileGetEvent(inputData);
                        break;
                }

            }



        }
        //All methods used to fire events
        #region All  event firer's
        private void FireEOF_Event()
        {
            if (EOF_Event != null)
                EOF_Event(this, null);
        }

        private void FireFileGetEvent(InputData data)
        {
            if (FileGetEvent != null)
                FileGetEvent(this, data);
        }

        private void FirePosEvent(InputData data)
        {
            if (TimePosEvent != null)
                TimePosEvent(this, data);
        }

        private void FireVolEvent(InputData data)
        {
            if (VolGetEvent != null)
                VolGetEvent(this, data);
        }
        #endregion

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
            timeRetVal = "Failure";
            timeResetEvent = new AutoResetEvent(false);
            TimePosEvent += posHandler;
            inStream.WriteLine("pausing_keep get_time_pos");
            timeResetEvent.WaitOne(5000);

            //Console.WriteLine("TimeRetVal: " + timeRetVal);
            return timeRetVal;
        }

        private void posHandler(object e, InputData args)
        {
            timeRetVal = args.Data;

            timeResetEvent.Set();
        }

        public void SetVolume(int pos)
        {
            if (pos <= 100 && pos >= 0)
                inStream.WriteLine("volume " + pos + " 1");
            else if (pos > 100)
                inStream.WriteLine("volume 100 1");
            else if (pos < 0)
                inStream.WriteLine("volume 0 1");
        }

        public void SetPosition(int pos)
        {
            if (pos <= 100 && pos >= 0)
                inStream.WriteLine("set_property percent_pos " + pos);
            else if (pos > 100)
                inStream.WriteLine("set_property percent_pos 100");
            else if (pos < 0)
                inStream.WriteLine("set_property percent_pos 1");
        }

        public string GetPaused()
        {
            return "";
        }

        public string GetVolume()
        {
            volRetval = "Failure";
            volResetEvent = new AutoResetEvent(false);
            inStream.WriteLine("pausing_keep get_property volume");
            VolGetEvent += volHandler;
            volResetEvent.WaitOne(5000);

            Console.WriteLine("VolRetVal: " + volRetval);
            return volRetval;
        }

        private void volHandler(object e, InputData args)
        {
            volRetval = args.Data;

            volResetEvent.Set();
        }

        public string GetPlayingFile()
        {

            fileRetval = "Failure";
            fileResetEvent = new AutoResetEvent(false);
            inStream.WriteLine("pausing_keep get_property path");
            FileGetEvent += fileGetHandler;
            fileResetEvent.WaitOne(5000);

            FileGetEvent -= fileGetHandler;
            return fileRetval;

        }

        private void fileGetHandler(object e, InputData args)
        {
            fileRetval = args.Data;

            fileResetEvent.Set();
        }
    }
}

