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
        private TCPUtillity util;
        private TCPHandle _handler;
        private Thread thread;

        private int _port;
        private string _localIp;

        public TCPReceiver(string localIp, int port)
        {
            _port = port;
            _localIp = localIp;
            listener = new TcpListener(IPAddress.Parse(_localIp), _port);

            listener.Start();

            _handler = new TCPHandle();
            thread = new Thread(run);
        }

        public void ConnectionSetup()
        {
            //TcpClient clientSocket = default(TcpClient);
            TcpClient clientSocket = listener.AcceptTcpClient();
            util = new TCPUtillity(clientSocket);
            Console.WriteLine("New connecton");
        }

        public void handler()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_handler.HandleHTTP), util);
        }

        public void start()
        {
            thread.Start();
        }

        public void run()
        {
            while (true)
            {
                ConnectionSetup();
                handler();
            }
        }
    }
}