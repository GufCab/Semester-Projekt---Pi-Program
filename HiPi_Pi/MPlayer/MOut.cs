using System;
using System.IO;
using System.Threading;


namespace MPlayer
{
    public class WrapperMOut
    {
        private StreamReader _output;
        private Thread outThread;

        public delegate void OutHandle(object e, InputData args);
        public event OutHandle outPut;

        public WrapperMOut()
        {
            outThread = new Thread(ReadFunc);
            outThread.Start();
        }

        public void SetOutStream(StreamReader stream)
        {
            if (stream != null)
            {
                _output = stream;
            }
        }

        private void ReadFunc()
        {
            string input = "";
            InputData sendData;

            while (true)
            {
                if (_output != null)
                {
                    while(_output.Peek() >= 0)
                        input = _output.ReadLine();
                }

                if (input != "")
                {
                    sendData = new InputData();
                    sendData.Data = input;

                    outPut(this, sendData);
                    input = "";
                }
            }
        }

        public class InputData : EventArgs
        {
            private string _data;

            public String Data
            {
                get { return _data; }
                set { _data = value; }
            }
        }
    }
}