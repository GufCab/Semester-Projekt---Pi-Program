using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace UPnP_Device
{
    public class TCPUtillity
    {
        private int BUFFERSIZE = 1000;
        private TcpClient _client;
        private NetworkStream _stream;

        public TCPUtillity(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            BUFFERSIZE = _client.ReceiveBufferSize;
        }

        public string TCPRecieve()
        {
            byte[] receiveBuffer = new byte[BUFFERSIZE];

            try
            {
                int size = _stream.Read(receiveBuffer, 0, BUFFERSIZE);

                _stream.Flush();

                return Encoding.UTF8.GetString(receiveBuffer, 0, size);
            }
            catch (Exception e)
            {
                Console.WriteLine("Buffer too small");
                Console.WriteLine(e.Message);
                return "-1";
            }
        }

        public void TCPSend(string msg)
        {
            _stream.Flush();

            byte[] sendBuffer = Encoding.UTF8.GetBytes(msg);

            _stream.Write(sendBuffer, 0, sendBuffer.Length);
        }
    }
}
