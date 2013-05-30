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
    /// <summary>
    /// This class is used to handle
    /// incoming Tcp connections and create 
    /// new threads, handling them.
    /// </summary>
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

        /// <summary>
        /// Constructor.
        /// Starts new thread containing the functionality.
        /// </summary>
        /// <param name="ipConf">IP configuration from factory</param>
        /// <param name="BasePath">Path to directory containing service and device descriptions</param>
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

        /// <summary>
        /// Accepts incoming connections and dispatches
        /// them to new threads, where they are handled 
        /// with functions defined in TCPHandle.
        /// </summary>
        private void ServerFunc()
        {
            welcomeSocket.Start();

            while (true)
            {
                TcpClient client = welcomeSocket.AcceptTcpClient();
                if(TCPDebug.MSG)
					Console.WriteLine("New TCP connection");
                Thread clientThread = new Thread(new ParameterizedThreadStart(tcpHandle.HandleHTTP));
                clientThread.Start(client);               
            }
        }
    }
}