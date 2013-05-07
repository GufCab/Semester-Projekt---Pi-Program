using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

//skal oprette et nyt tcphandler objekt som starten ny tråd der håndterer HTTP beskeden når der modtages noget

namespace UPnP_Device
{
    public class TCPReceiver
    {
        public const int BUFFER_SIZE = 900000; //set size of handler buffer

        private TcpListener listener;
        //private TcpClient clientSocket;
        private NetworkStream networkStream;
        //private TCPUtillity util;
        private TCPHandle _handler;
        private Thread thread;

        private int _port;
        private string _localIp;

        public TCPReceiver(string localIp, int port)
        {
            _port = port;
            _localIp = localIp;
            //listener = new TcpListener(IPAddress.Parse(_localIp), _port);
            listener = new TcpListener(IPAddress.Any, _port);

            _handler = new TCPHandle();
            thread = new Thread(run);
        }

        public TCPUtillity ConnectionSetup()
        {
            Console.WriteLine("Ready for TCP connection");
            TcpClient clientSocket = listener.AcceptTcpClient();
            Console.WriteLine("TCP Connection accepted");
            
            clientSocket.ReceiveTimeout = 5000;
            
            return new TCPUtillity(clientSocket);
        }

        public void handler(TCPUtillity util)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_handler.HandleHTTP), util);
        }

        public void start()
        {
            listener.Start();
            thread.Start();
        }

        public void run()
        {
            while (true)
            {
                TCPUtillity util = ConnectionSetup();
                handler(util);
                
            }
        }
    }
}