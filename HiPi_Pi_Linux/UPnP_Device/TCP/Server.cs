using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UPnP_Device.UPnPConfig;

namespace UPnP_Device.TCP
{
    //Todo: Filename should be TCPServer
    public class TcpServer
    {
        //Todo: Buffersize isn't used:
        private int BUFFER_SIZE = 90000;
        private TcpListener welcomeSocket;
        private TCPHandle tcpHandle;
        private Thread serverThread;

        //Todo: apperently variables never used:
        private int localPort;
        private string localIp;

		public TcpServer(IIpConfig ipConf, string BasePath)
        {
            localIp = ipConf.IP;
            localPort = ipConf.TCPPort;

            tcpHandle = new TCPHandle(BasePath, ipConf);

            //Todo: Any IPAddress and a port. Is this an EndPoint?
            //Todo: Should we listen at our own local IP only?
            welcomeSocket = new TcpListener(IPAddress.Any, localPort);
            serverThread = new Thread(ServerFunc);
            serverThread.Start();
        }

        private void ServerFunc()
        {
            welcomeSocket.Start();

            //Todo: Nice, MDS.. Comments needed
            while (true)
            {
                Console.WriteLine("Blocking");
                TcpClient client = welcomeSocket.AcceptTcpClient();
                Console.WriteLine("New connection");
                Thread clientThread = new Thread(new ParameterizedThreadStart(tcpHandle.HandleHTTP));
                clientThread.Start(client);               
            }
        }
    }
}