using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

//skal oprette et nyt tcphandler objekt som starten ny tråd der håndterer HTTP beskeden når der modtages noget

namespace UPnP_Device
{
    class TCPServer
    {
        public const int BUFFER_SIZE = 900000;			//set size of receive buffer

        public TcpListener serverSocket;
        public TcpClient clientSocket;
        public NetworkStream clientStream;

        public byte[] outStream;
        public byte[] inStream = new byte[BUFFER_SIZE];
        public string inString = "";
        private int _port;

        public TCPServer(int port)
        {
            _port = port;
            ConnectionSetup();
            ReceiveTCPMessage();
        }

        public void ConnectionSetup()
        {

            serverSocket = new TcpListener(IPAddress.Any, _port);
    
            serverSocket.Start();

            //Creates client socket:
            //clientSocket = default(TcpClient);
            clientSocket = serverSocket.AcceptTcpClient();
            clientStream = clientSocket.GetStream();
        }

        //Receive from client:
        public String ReceiveTCPMessage()
        {
            try
            {
                clientStream.Flush();		//Clear stream
                clientStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);		//Read from Stream
                inString = System.Text.Encoding.ASCII.GetString(inStream);		//Format string
                return inString;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at reception:");
                Console.WriteLine(e.Message);
                return "-1";
            }
        }

        //Send to client:
        public bool SendTCPMessage(string msg)
        {
            try
            {
                clientStream.Flush();		//Clear stream
                outStream = System.Text.Encoding.ASCII.GetBytes(msg);
                clientStream.Write(outStream, 0, outStream.Length);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at transmission:");
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
