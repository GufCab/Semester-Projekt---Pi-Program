using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace FileSenderServer
{
    /// <summary>
    /// Abstract "interface" for FileSenderServer
    /// </summary>
    public abstract class AbstractFileSenderServer
    {
        protected abstract string LocalIpAddress();
        protected abstract void SetUp();
        protected abstract void ReadFileName();
        protected abstract void ReadFileSize();
        protected abstract void ReceiveFile(String fileName, NetworkStream io);
        protected abstract void CloseSocketConnection();
        public abstract void Dispose();
        protected abstract void Run();

    }
}
