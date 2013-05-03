using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

//skal oprette et nyt tcphandler objekt som starten ny tråd der håndterer HTTP beskeden når der modtages noget

namespace UPnP_Device
{
    public class TCPReceiver
    {
        public const int BUFFER_SIZE = 900000; //set size of receive buffer

        private TcpClient clientSocket;
        private NetworkStream networkStream;

        private int _port;
        private string _localIp;

        public TCPReceiver(string localIp, int port)
        {
            _port = port;
            _localIp = localIp;
            ConnectionSetup();
            ReceiveTCPMessage();
        }

        public void ConnectionSetup()
        {
            clientSocket = new TcpClient(_localIp, _port);
            networkStream = clientSocket.GetStream();

        }

        //Receive from client:
        public void ReceiveTCPMessage()
        {
            try
            {
                clientStream.Flush(); //Clear stream
                clientStream.Read(inStream, 0, (int) clientSocket.ReceiveBufferSize); //Read from Stream
                inString = System.Text.Encoding.ASCII.GetString(inStream); //Format string
                TCPHandle tcpHandle = new TCPHandle(inString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at reception:");
                Console.WriteLine(e.Message);
            }
        }
    }
}