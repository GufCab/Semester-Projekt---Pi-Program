using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace UPnP_Device
{
    class TCPUtillity
    {
        private const int BUFFERSIZE = 1000;

        public string TCPRecieve(NetworkStream stream)
        {
            byte[] receiveBuffer = new byte[BUFFERSIZE];

            try
            {
                stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                return Encoding.UTF8.GetString(receiveBuffer, 0, receiveBuffer.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Buffer too small");
                Console.WriteLine(e.Message);
                return "-1";
            }
        }

        public void TCPSend(NetworkStream stream, string msg)
        {
            byte[] sendBuffer = Encoding.ASCII.GetBytes(msg);

            stream.Write(sendBuffer, 0, sendBuffer.Length);
        }
    }
}
