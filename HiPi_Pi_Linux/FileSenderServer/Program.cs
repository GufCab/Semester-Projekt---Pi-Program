using System;

namespace FileSenderServer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            do
            {
                try
                {

                    using (var server = new Server())
                    {
                        server.SetUp();
                        server.ReadFileName();
                        server.ReadFileSize();
                        server.ReceiveFile(server._fileName, server._serverStream);
                        server.CloseSocketConnection();
                        Console.WriteLine();
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            } while (true);
        }
    }
}
