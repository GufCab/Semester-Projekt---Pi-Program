using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// Namespace for FileSenderServer.
/// </summary>
namespace FileSenderServer
{
    /// <summary>
    /// Author: Michael Thy Oksen, 11492@iha.dk.
    /// Description: This class is responsible for handling the task of receiving files from the Client (The PC) to the Server (The Raspberry Pi).
    /// </summary>
    public class FileSenderServer : AbstractFileSenderServer, IDisposable
    {
        public IPAddress _IP { get; private set; }
        public TcpListener _serverSocket { get; private set; }
        public TcpClient _clientSocket { get; private set; }
        public NetworkStream _serverStream { get; private set; }
        public string _fileName { get; private set; }
        public string _fileSize { get; private set; }

        /// <summary>
        /// Sets the constructor to start a new thread running the Run() function.
        /// </summary>
        public FileSenderServer()
        {
                var thread = new Thread(Run);
                thread.Start();
        }

        /// <summary>
        /// This method gets an array of the IP-addresses that the system uses.
        /// </summary>
        /// <returns>
        /// A string containing an IP.
        /// </returns>
        protected override string LocalIpAddress()
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

        /// <summary>
        /// This method is responsible for setting up the required variables and external classes.
        /// </summary>
        protected override void SetUp()
        {
            try
            {
                _IP = IPAddress.Parse(LocalIpAddress());                                            //Convert tempIP to IP
                _serverSocket = new TcpListener(_IP, PORT);                                         //Create and initialize TCPlistener
                _clientSocket = new TcpClient();
                _clientSocket = default(TcpClient);
                _serverSocket.Start();                                                              //Start listening on serverSocket
                ////For testing purpose only
                //Console.WriteLine(" >> TCP server started - Listening on port {0}...", PORT);       //Write which port we're listening to
                //Console.WriteLine(" >> The IP:Port is: {0}:{1}", LocalIpAddress(), PORT);           //Write local end point
                //Console.WriteLine(" >> Waiting for connection.....");                               //Indicate server is waiting for connection
                //Console.WriteLine();
                _clientSocket = _serverSocket.AcceptTcpClient();                                    //Set server to accept connections
                _serverStream = _clientSocket.GetStream();                                          //Prepare to receive file name
                ////For testing purpose only
                //if (_clientSocket != null)
                //{
                //    Console.WriteLine(" >> TCP server connected to {0} on port {1}...",
                //                        _clientSocket.Client.LocalEndPoint, PORT);                    //Tell user that client is started on chosen port
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} happened!", e.Data);
                throw;
            }
        }
        
        /// <summary>
        /// Receives the file name through TCP and sets it.
        /// </summary>
        protected override void ReadFileName()
        {
            _fileName = Path.GetFileName(LIB.readTextTCP(_serverStream)); //Read file name
            ////For testing purpose only
            //Console.WriteLine("File name is: '{0}'", Path.GetFileName(_fileName)); //Tell the user the file name
        }

        /// <summary>
        /// Receives the file size through TCP and sets it.
        /// </summary>
        protected override void ReadFileSize()
        {
            _fileSize = LIB.readTextTCP(_serverStream); //Read file size
            ////For testing purpose only
            //Console.WriteLine("File size is: {0} bytes.", _fileSize); //Output file size to console
        }

        /// <summary>
        /// The PORT
        /// </summary>
        private const int PORT = 9003;
        /// <summary>
        /// The BUFSIZE
        /// </summary>
        private const int BUFSIZE =  5000 * 8;

        /// <summary>
        /// Receives the file sequantially.
        /// </summary>
        /// <param name="fileName">File name of the file-to-be-received</param>
        /// <param name="io"></param>
        protected override void ReceiveFile(String fileName, NetworkStream io)
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
                ////For testing purpose only
                //Console.WriteLine("Remaining number of bytes: {0}", remainingSize);
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

        /// <summary>
        /// This method closes the socket connection to the client.
        /// </summary>
        protected override void CloseSocketConnection()
        {
            _clientSocket.Close();
        }
        /// <summary>
        /// Implemented so usage of "using(xxxxxx)" is possible. 
        /// </summary>
        public override void Dispose()
        {
            _serverSocket.Stop();
            _clientSocket.Close();
        }

        /// <summary>
        /// Runs Sequentially through the process of receiving the file as bytes.
        /// </summary>
        protected override void Run()
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
