using System;
using System.IO;
using System.Threading;

namespace MPlayer
{
    public class MPlayerOut
    {
        private StreamReader playerStream;
        private Thread readerThread;

        public delegate void OutHandle(object e, InputData args);
        public event OutHandle outPut;

        InputData input = null;


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
    }

    public class InputData : EventArgs
    {
        public string Data { get; private set; }

        public InputData(string data)
        {
            Data = data;
        }
    }
}

