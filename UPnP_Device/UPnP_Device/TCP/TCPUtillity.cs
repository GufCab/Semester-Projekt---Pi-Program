﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace UPnP_Device
{
    public class TCPUtillity
    {
        private int BUFFERSIZE = 9000000;
        private TcpClient _client;
        private NetworkStream _stream;

        public TCPUtillity(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            //BUFFERSIZE = _client.ReceiveBufferSize;
        }

        public string TCPRecieve()
        {
            BUFFERSIZE = (_client.ReceiveBufferSize+1);
            Console.WriteLine("Buffer size: " + BUFFERSIZE);
            byte[] receiveBuffer = new byte[BUFFERSIZE];

            try
            {
                _stream.Flush();
                
                int size = 0;

                if (_stream.DataAvailable)
                {
                    size = _stream.Read(receiveBuffer, 0, BUFFERSIZE);
                    Console.WriteLine("Stream read");
                }
                    
                else
                {
                    Console.WriteLine("Data not available. Closing socket..");
                    _stream.Close();
                    _client.Close();
                }
                
                
                _stream.Flush();
                 
                return Encoding.UTF8.GetString(receiveBuffer, 0, size);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                Console.WriteLine("");
                throw;
            }
        }

        public void TCPClose()
        {
            _stream.Close();
            _client.Close();
        }

        public void TCPSend(string msg)
        {
            _stream.Flush();

            byte[] sendBuffer = Encoding.UTF8.GetBytes(msg);

            _stream.Write(sendBuffer, 0, sendBuffer.Length);
            Console.WriteLine("TCP Message sent");
        }
    }
}
