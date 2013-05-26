using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MPlayer;

namespace MPlayer
{
    /// <summary>
    /// This class serves as the wrapper/adapter for the MPlayer Process.
    /// MPlayerWrapper implements the interface IWrapper and is responsible
    /// for creating and starting the MPlayer Process. 
    /// 
    /// In a future Refactoring, some functionality of this class will be 
    /// delegated to smaller sub-classes. 
    /// </summary>
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

		private AutoResetEvent pauseResetEvent;
		private string pauseRetval;

        public delegate void TimePosHandle(object e, InputData args);
        public event TimePosHandle TimePosEvent;

        public delegate void VolGetHandle(object e, InputData args);
        public event VolGetHandle VolGetEvent;

        public delegate void FileGetHandle(object e, InputData args);
        public event FileGetHandle FileGetEvent;

        public delegate void EOFHandle(object e, EventArgs args);
        public event EOFHandle EOF_Event;

        public delegate void PauseHandle(object e, InputData args);
        public event PauseHandle PauseGetEvent;

        /// <summary>
        /// Constructor. Sets up the MPlayer Process with the 
        /// correct arguments and startup-parameters. 
        ///  
        /// Creates an instance of MPlayerOut.
        /// </summary>
        public MPlayerWrapper()
        {
            SetupMPlayer();

            mout = new MPlayerOut(outStream);
            Subscribe();
        }

        /// <summary>
        /// Sets up MPlayer with correct startup Arguments.
        /// Startup arguments are:
        /// -idle
        ///     MPlayer will not kill itself when a song ends
        ///     but instead sit in idle and wait for next song
        /// -quiet
        ///     MPlayer will only output the neccessary information in 
        ///     the redirected Console. What is outputted is specified
        ///     in MPlayers Configuration file.
        /// -slave
        ///     MPlayer is running as a slave to the main program.
        ///     It will accept slave commands.
        /// 
        /// Also redirects the StdOut, StdIn and StdErr streams and
        /// sets in and out streams to outStream and inStream, wich
        /// will be used in the program. 
        /// </summary>
        private void SetupMPlayer()
        {
            mplayer = new Process();
            var startInfo = new ProcessStartInfo();

            string arguments = "-idle -quiet -slave";

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

        /// <summary>
        /// MPlayerWrapper will subscribe to the mout instance
        /// of MPlayerOut's OutPut event.
        /// </summary>
        private void Subscribe()
        {
            mout.outPut += new MPlayerOut.OutHandle(OutputEventHandler);
        }

        /// <summary>
        /// Handler for MPlayerOut's OutputEvent.
        /// </summary>
        /// <param name="e">Sender</param>
        /// <param name="args">InputData object containing MPlayers output.</param>
        private void OutputEventHandler(object e, InputData args)
        {
            var dataArray = InterpreterClass.DataToArray(args.Data);
            FireEvents(dataArray);
        }

        /// <summary>
        /// Determines which event to raise, bases on what was outputted from MPlayerOut.
        /// First index of the arr param is the output type.
        /// Second index of the arr param is the output value.
        /// </summary>
        /// <param name="arr">Data outputted from MPlayerOut as a string array</param>
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
					case "ANS_pause":
						FirePauseEvent(inputData);
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

		private void FirePauseEvent(InputData data)
        {
            if (PauseGetEvent != null)
                PauseGetEvent(this, data);
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

        /// <summary>
        /// Orders MPlayer to play at path specified by the path param.
        /// The replace action used on the path variable, is to replace
        /// whitespaces in the path with escaped whitespaces.
        /// </summary>
        /// <param name="path">path to start playing from</param>
        public void PlayTrack(string path)
        {
			string order = "loadfile " + path.Replace(" ", "\\ ");
            inStream.WriteLine(order);
        }

        /// <summary>
        /// Pause current track
        /// </summary>
        public void PauseTrack()
        {
            inStream.WriteLine("pause");
        }

        /// <summary>
        /// Asks MPlayer to output the current position in the playing track.
        /// Waits 5 seconds for response, if no response return 0.
        /// </summary>
        /// <returns>Position in seconds as a string.</returns>
        public string GetPosition()
        {
            timeRetVal = "0";
            timeResetEvent = new AutoResetEvent(false);
            TimePosEvent += posHandler;
            inStream.WriteLine("pausing_keep get_time_pos");
            timeResetEvent.WaitOne(5000);
			string[] ret = timeRetVal.Split('.');

            return ret[0];
        }

        private void posHandler(object e, InputData args)
        {
            timeRetVal = args.Data;

            timeResetEvent.Set();
        }

        /// <summary>
        /// Set volume of MPlayer
        /// </summary>
        /// <param name="pos">Volume in range 0 - 100</param>
        public void SetVolume(int pos)
        {
            if (pos <= 100 && pos >= 0)
                inStream.WriteLine("volume " + pos + " 1");
            else if (pos > 100)
                inStream.WriteLine("volume 100 1");
            else if (pos < 0)
                inStream.WriteLine("volume 0 1");
        }

        /// <summary>
        /// Change position in current playing.
        /// </summary>
        /// <param name="pos">Position in 0% - 100%</param>
        public void SetPosition(int pos)
        {
            if (pos <= 100 && pos >= 0)
                inStream.WriteLine("set_property percent_pos " + pos);
            else if (pos > 100)
                inStream.WriteLine("set_property percent_pos 100");
            else if (pos < 0)
                inStream.WriteLine("set_property percent_pos 1");
        }
        
        /// <summary>
        /// Returns paused state as bool.
        /// True if paused, false if not.
        /// </summary>
        /// <returns>Paused state as bool</returns>
        public bool GetPaused ()
		{
			pauseRetval = "Failure";
			pauseResetEvent = new AutoResetEvent (false);

			inStream.WriteLine ("pausing_keep_force get_property paused");
			PauseGetEvent += pausedHandler;
			pauseResetEvent.WaitOne (5000);

			if (pauseRetval == "Failure")
				return true;

			return pauseRetval.Contains("1");
        }

		private void pausedHandler (object e, InputData args)
		{
			pauseRetval = args.Data;

			pauseResetEvent.Set();
		}

        /// <summary>
        /// Asks MPlayer to output the current volume.
        /// Waits 5 seconds for response, if no response return 0.
        /// </summary>
        /// <returns>Volume as int in range 0 - 100</returns>
        public string GetVolume()
        {
            volRetval = "0";
            volResetEvent = new AutoResetEvent(false);
            inStream.WriteLine("pausing_keep get_property volume");
            VolGetEvent += volHandler;
            volResetEvent.WaitOne(5000);
			var ret = volRetval.Split('.');

            Console.WriteLine("VolRetVal: " + ret[0]);
            return ret[0];
        }

        private void volHandler(object e, InputData args)
        {
            volRetval = args.Data;

            volResetEvent.Set();
        }

        /// <summary>
        /// Asks MPlayer to output the current path of playing track.
        /// Waits 5 seconds for response, if no response return "Failure".
        /// </summary>
        /// <returns>Path to current playing file</returns>
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

