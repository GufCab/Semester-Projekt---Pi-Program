using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
/*
namespace UPnP_Device
{
    class TCPSender
    {
       public const int BUFFER_SIZE = 900000; //set size of receive buffer

        private TcpClient clientSocket;
        private NetworkStream networkStream;
        private TCPUtillity util;
        private TCPHandle _handler;

        private int _port;
        private string _localIp;

        public TCPSender(string localIp, int port, TCPHandle handler)
        {
            _port = port;
            _localIp = localIp;
            ConnectionSetup();
            util = new TCPUtillity();
            _handler = handler;
        }

        public void ConnectionSetup()
        {
            clientSocket = new TcpClient(_localIp, _port);
            networkStream = clientSocket.GetStream();
        }

        public void send()
        {
            //receives a message and creates a TCPHandle to handle the message
            object[] handleObj = new object[2];
            handleObj[0] = networkStream;
            handleObj[1] = util.TCPRecieve(networkStream);
            
            ThreadPool.QueueUserWorkItem(new WaitCallback(_handler.HandleHTTP), handleObj);
        }
    }
*/