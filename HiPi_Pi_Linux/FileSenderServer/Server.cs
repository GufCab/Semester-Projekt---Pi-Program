using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FileSenderServer
{
    public class Server : IServer, IDisposable
    {
        public IPAddress _IP { get; private set; }
        public TcpListener _serverSocket { get; private set; }
        public TcpClient _clientSocket { get; private set; }
        public NetworkStream _serverStream { get; private set; }
        public string _fileName { get; private set; }
        public string _fileSize { get; private set; }

        public Server()
        {
                var thread = new Thread(Run);
                thread.Start();
        }

        private string LocalIpAddress()
        {
            string localIp = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                }
            }
            return localIp;
        }

        private void SetUp()
        {
            try
            {
                _IP = IPAddress.Parse(LocalIpAddress());                                            //Convert tempIP to IP
                _serverSocket = new TcpListener(_IP, PORT);                                         //Create and initialize TCPlistener
                _clientSocket = new TcpClient();
                _clientSocket = default(TcpClient);
                _serverSocket.Start();                                                              //Start listening on serverSocket
                Console.WriteLine(" >> TCP server started - Listening on port {0}...", PORT);       //Write which port we're listening to
                Console.WriteLine(" >> The IP:Port is: {0}:{1}", LocalIpAddress(), PORT);           //Write local end point
                Console.WriteLine(" >> Waiting for connection.....");                               //Indicate server is waiting for connection
                Console.WriteLine();
                _clientSocket = _serverSocket.AcceptTcpClient();                                    //Set server to accept connections
                _serverStream = _clientSocket.GetStream();                                          //Prepare to receive file name
                if (_clientSocket != null)
                {
                    Console.WriteLine(" >> TCP server connected to {0} on port {1}...",
                                        _clientSocket.Client.LocalEndPoint, PORT);                    //Tell user that client is started on chosen port
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} happened!", e.Data);
                throw;
            }
        }
        
        private void ReadFileName()
        {
            _fileName = Path.GetFileName(LIB.readTextTCP(_serverStream)); //Read file name
            Console.WriteLine("File name is: '{0}'", Path.GetFileName(_fileName)); //Tell the user the file name
        }

        private void ReadFileSize()
        {
            _fileSize = LIB.readTextTCP(_serverStream); //Read file size
            Console.WriteLine("File size is: {0} bytes.", _fileSize); //Output file size to console
        }

        /// <summary>
        /// The PORT
        /// </summary>
        private const int PORT = 9003;
        /// <summary>
        /// The BUFSIZE
        /// </summary>
        private const int BUFSIZE =  5000 * 8;

        private void ReceiveFile(String fileName, NetworkStream io)
        {
            // TO DO Din egen kode
            byte[] fileData = new byte[BUFSIZE];
            if (!Directory.Exists("./FileSender Music/"))
            {
                Directory.CreateDirectory("./FileSender Music/");
            }
            FileStream writeFileStream = new FileStream("./FileSender Music/" + LIB.extractFileName(fileName), FileMode.Create);

            BinaryWriter bWrite = new BinaryWriter(writeFileStream);

            int bytesRead = 0;
            long remainingSize = Convert.ToInt32(_fileSize);

            do
            {
                Console.WriteLine("Remaining number of bytes: {0}", remainingSize);
                if (Convert.ToInt32(_fileSize) >= BUFSIZE)
                {
                    bytesRead = io.Read(fileData, 0, BUFSIZE);
                        // Read max 5000 bytes from server via socket (actual value is placed in "bytesRead"
                    bWrite.Write(fileData, 0, bytesRead);
                        // write the received bytes into file. the number of received bytes is placed in "bytesRead"
                    remainingSize -= bytesRead;
                }
                else if (Convert.ToInt32(_fileSize) < BUFSIZE)
                {
                    bytesRead = io.Read(fileData, 0, Convert.ToInt32(remainingSize));
                    // Read max 5000 bytes from server via socket (actual value is placed in "bytesRead"
                    bWrite.Write(fileData, 0, bytesRead);
                    // write the received bytes into file. the number of received bytes is placed in "bytesRead"
                    remainingSize -= bytesRead;
                }
            }
            while (remainingSize > 0);
            writeFileStream.Flush();
            writeFileStream.Close();
            bWrite.Close();
        }

        private void CloseSocketConnection()
        {
            _clientSocket.Close();
        }

        public void Dispose()
        {
            _serverSocket.Stop();
            _clientSocket.Close();
        }

        public void Run()
        {
            try
            {
                while (true)
                {
                    SetUp();
                    ReadFileName();
                    ReadFileSize();
                    ReceiveFile(_fileName, _serverStream);
                    Thread.Sleep(10);
                    CloseSocketConnection();
                    _serverSocket.Stop();
                    Console.WriteLine();
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
