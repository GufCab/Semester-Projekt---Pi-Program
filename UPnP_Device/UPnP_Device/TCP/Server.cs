using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPnP_Device.TCP
{
    public class TcpServer
    {
        private int BUFFER_SIZE = 90000;
        private TcpListener welcomeSocket;
        private TCPHandle tcpHandle;
        private Thread serverThread;

        private int localPort;
        private string localIp;

        public TcpServer(string ip, int port)
        {
            localIp = ip;
            localPort = port;

            tcpHandle = new TCPHandle();

            welcomeSocket = new TcpListener(IPAddress.Any, port);
            serverThread = new Thread(ServerFunc);
            serverThread.Start();
        }

        private void ServerFunc()
        {
            welcomeSocket.Start();

            while (true)
            {
                TcpClient client = welcomeSocket.AcceptTcpClient();

                Thread clientThread = new Thread(new ParameterizedThreadStart(tcpHandle.HandleHTTP));
                clientThread.Start(client);               
            }
        }
    }
}