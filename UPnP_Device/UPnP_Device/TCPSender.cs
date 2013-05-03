using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UPnP_Device
{
    class TCPSender
    {
        public const int BUFFER_SIZE = 900000; //set size of receive buffer

        public TcpListener serverSocket;
        public TcpClient clientSocket;
        public NetworkStream clientStream;

        public byte[] outStream;
        public byte[] inStream = new byte[BUFFER_SIZE];
        public string outString = "";
        private int _port;
        private string _localIp;

        public TCPSender(string localIp, int port)
        {
            _port = port;
            _localIp = localIp;
            ConnectionSetup();
            SendTCPMessage(outString);
        }

        public void ConnectionSetup()
        {
            serverSocket = new TcpListener(IPAddress.Parse(_localIp), _port);

            serverSocket.Start();

            //Creates client socket:
            //clientSocket = default(TcpClient);
            clientSocket = serverSocket.AcceptTcpClient();
            clientStream = clientSocket.GetStream();
        }

        public bool SendTCPMessage(string msg)
        {
            try
            {
                clientStream.Flush(); //Clear stream
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



    /*
//Send to client:
public bool SendTCPMessage(string msg)
{
    try
    {
        clientStream.Flush(); //Clear stream
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
}*/
}
