using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using UPnP_Device.TCP;

namespace UPnP_Device
{
    /// <summary>
    /// Interface for NetworkUtility
    /// </summary>
    public interface INetworkUtillity
    {
        string Receive();
        void Close();
        void Send(string msg);
        bool IsConnected();
    }

    /// <summary>
    /// Used as a wrapper for TcpClient,
    /// easing access by taking care of reading and writing 
    /// to the client. 
    /// </summary>
    public class NetworkUtillity : INetworkUtillity
    {
        private int BUFFERSIZE = 9000000;
        private TcpClient _client;
        private NetworkStream _stream;
        public int TimeOut { get; set; }

        /// <summary>
        /// How long to wait before connection should time out.
        /// </summary>
        public int TIMEOUT
        {
            get { return _client.ReceiveTimeout; }
            set { _client.ReceiveTimeout = value*1000; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client containing an accepted connection to Control Point</param>
        public NetworkUtillity(TcpClient client)
        {
            _client = client;
            TIMEOUT = 5;
            _stream = _client.GetStream();
            //BUFFERSIZE = _client.ReceiveBufferSize;
        }

        /// <summary>
        /// Receive function to read the incoming stream
        /// from the socket.
        /// </summary>
        /// <returns>The entire string read from the stream</returns>
        public string Receive()
        {
            BUFFERSIZE = (_client.ReceiveBufferSize+1);
			if(TCPDebug.DEBUG) 
            	Console.WriteLine("Buffer size: " + BUFFERSIZE);
            byte[] receiveBuffer = new byte[BUFFERSIZE];

            try
            {
                Socket s = _client.Client;
                if(TCPDebug.DEBUG) {
					Console.WriteLine("Remote end:" + s.RemoteEndPoint.ToString());
					Console.WriteLine("Local : " + s.LocalEndPoint.ToString());
				}
                
               // _stream.Flush();

				if(TCPDebug.DEBUG) { Console.WriteLine("Data available: " + _stream.DataAvailable);}

                int size = 0;

                size = _stream.Read(receiveBuffer, 0, BUFFERSIZE);

				if(TCPDebug.DEBUG) 
                	Console.WriteLine("Stream read");
                _stream.Flush();

                string msg = Encoding.UTF8.GetString(receiveBuffer, 0, size);

				if(TCPDebug.DEBUG) { Console.WriteLine("Received TCP: " + msg);}

                return msg;
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

        /// <summary>
        /// Check if the client is connected
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return _client.Connected;
        }

        /// <summary>
        /// Close client socket
        /// </summary>
        public void Close()
        {
            _stream.Close();
            _client.Close();
        }

        /// <summary>
        /// Used to send a string to the Control Point
        /// </summary>
        /// <param name="msg">String to be send</param>
        public void Send(string msg)
        {
			if(TCPDebug.DEBUG) {Console.WriteLine("Sending msg: " + msg);}
            _stream.Flush();

            byte[] sendBuffer = Encoding.UTF8.GetBytes(msg);

            _stream.Write(sendBuffer, 0, sendBuffer.Length);
			if(TCPDebug.MSG)
            	Console.WriteLine(">> TCP Message sent");
        }
    }
}
