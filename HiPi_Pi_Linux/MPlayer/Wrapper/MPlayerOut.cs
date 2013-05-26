using System;
using System.IO;
using System.Threading;

namespace MPlayer
{
    /// <summary>
    /// Class responsible for reading from the output stream 
    /// of MPlayer.
    /// </summary>
    public class MPlayerOut
    {
        private StreamReader playerStream;
        private Thread readerThread;

        public delegate void OutHandle(object e, InputData args);
        public event OutHandle outPut;

        InputData input = null;

        /// <summary>
        /// Starts the reading process in a new thread.
        /// </summary>
        /// <param name="stream">MPlayers Redirected output stream</param>
        public MPlayerOut(StreamReader stream)
        {
            playerStream = stream;
            readerThread = new Thread(ReaderFunc);
            readerThread.Start();
        }

        private void ReaderFunc()
        {
            string ValFromStream = "";

            while (true)
            {
                if (playerStream.Peek() >= 0)
                {
                    ValFromStream = playerStream.ReadLine();
                    input = new InputData(ValFromStream);

                    if (outPut != null)
                        outPut(this, input);
                }
            }
        }

        /// <summary>
        /// Get method to get MPlayers output stream
        /// </summary>
        /// <returns>MPlayers Redirected output stream</returns>
        public StreamReader GetStream()
        {
            return playerStream;
        }
    }

    /// <summary>
    /// Container class for the OutPut event
    /// containing what data MPlayer wrote to 
    /// the redirected output stream.
    /// </summary>
    public class InputData : EventArgs
    {
        public string Data { get; private set; }

        public InputData(string data)
        {
            Data = data;
        }
    }
}

