using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace UPnP_Device
{
    public interface INetworkUtillity
    {
        string Receive();
        void Close();
        void Send(string msg);
    }

    public class NetworkUtillity : INetworkUtillity
    {
        private int BUFFERSIZE = 9000000;
        private TcpClient _client;
        private NetworkStream _stream;
        public int TimeOut { get; set; }

        public int TIMEOUT
        {
            get { return _client.ReceiveTimeout; }
            set { _client.ReceiveTimeout = value*1000; }
        }

        public NetworkUtillity(TcpClient client)
        {
            _client = client;
            TIMEOUT = 5;
            _stream = _client.GetStream();
            //BUFFERSIZE = _client.ReceiveBufferSize;
        }

        public string Receive()
        {
            //BUFFERSIZE = (_client.ReceiveBufferSize+1);
            Console.WriteLine("Buffer size: " + BUFFERSIZE);
            byte[] receiveBuffer = new byte[BUFFERSIZE];

            try
            {
                _stream.Flush();

                Console.WriteLine("Data available: " + _stream.DataAvailable);

                int size = _stream.Read(receiveBuffer, 0, BUFFERSIZE);

                Console.WriteLine("Stream read");
                _stream.Flush();

                return Encoding.UTF8.GetString(receiveBuffer, 0, size);
            }
            catch (IOException e)
            {
                Close();
                Console.WriteLine(e.Message);
                return "-1";
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                //throw;
                return "-1";
                
            }

            
        }

        public void Close()
        {
            _stream.Close();
            _client.Close();
        }

        public void Send(string msg)
        {
            _stream.Flush();

            byte[] sendBuffer = Encoding.UTF8.GetBytes(msg);

            _stream.Write(sendBuffer, 0, sendBuffer.Length);
            Console.WriteLine("TCP Message sent");
        }
    }
}
