using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UPnP_Device.UPnPConfig
{
    /// <summary>
    /// Interface for IIpConfig.
    /// Exposes Get/Set methods for IP, GUID and TCPPort.
    /// 
    /// Created by UPnP factory and passed to UPnPDevice to configure
    /// the IP and ID related properties of the device.
    /// </summary>
    public interface IIpConfig
    {
        string GUID { get; }
        string IP { get; }

        int TCPPort { get; }
    }
    /// <summary>
    /// Implements the IIpConfig interface.
    /// 
    /// Created by UPnP factory and passed to UPnP sink devices to configure
    /// the IP and ID related properties of the device.
    /// </summary>
    public class SinkIPConfig : IIpConfig
    {
        public string GUID { get; private set; }
        public string IP { get; private set; }
        public int TCPPort { get; private set; }

        /// <summary>
        /// Constructor
        /// Takes a network port as argument.
        /// Gets the devices own IP and GUID through its own methods.
        /// </summary>
        /// <param name="tcpportnr">Network Port</param>
        public SinkIPConfig(int tcpportnr)
        {
            GUID = GetGUID();
            IP = GetOwnIp();
            TCPPort = tcpportnr;
        }

        /// <summary>
        /// Method to get a GUID.
        /// If the SinkGUID.key file exists in the relative path ../../config/
        /// the function will read this file and return the value read, thus 
        /// ensuring the GUID is kept through reboot.
        /// 
        /// If the file does not exist, a new GUID is created and stored in a file
        /// for later use.
        /// </summary>
        /// <returns>GUID for the device</returns>
        private string GetGUID()
        {
			string g;
            const string folderpath = @"../../config/";
            const string filename = "SinkGUID.key";
            const string fullpath = folderpath + filename;

            if (File.Exists(fullpath))
            {
                using (var sr = new StreamReader(fullpath, Encoding.UTF8))
                {
                    g = sr.ReadLine();
                    sr.Close();
                }
            }
            else
            {
                g = Guid.NewGuid().ToString();

                if (Directory.Exists(folderpath) == false)
                    Directory.CreateDirectory(folderpath);
                using (var fs = new FileStream(fullpath, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(g);
                    sw.Close();
                }
            }
            return g; //"e22ca7c1-aadc-4d60-b334-2d905bef5be7";
        }

        /// <summary>
        /// Method to get the devices own IP adress.
        /// Runs through all the devices IP's and selects one used for InterNetwork.
        /// </summary>
        /// <returns>IP Adress as a string</returns>
        private string GetOwnIp()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

    }

    /// <summary>
    /// Implements the IIpConfig interface.
    /// 
    /// Created by UPnP factory and passed to UPnP source devices to configure
    /// the IP and ID related properties of the device.
    /// </summary>
    public class SourceIPConfig : IIpConfig
    {
        public string GUID { get; private set; }
        public string IP { get; private set; }
        public int TCPPort { get; private set; }

        /// <summary>
        /// Constructor
        /// Takes a network port as argument.
        /// Gets the devices own IP and GUID through its own methods.
        /// </summary>
        /// <param name="tcpportnr">Network Port</param>
        public SourceIPConfig(int tcpportnr)
        {
            GUID = GetGUID();
            IP = GetOwnIp();
            TCPPort = tcpportnr;
        }

        /// <summary>
        /// Method to get a GUID.
        /// If the SinkGUID.key file exists in the relative path ../../config/
        /// the function will read this file and return the value read, thus 
        /// ensuring the GUID is kept through reboot.
        /// 
        /// If the file does not exist, a new GUID is created and stored in a file
        /// for later use.
        /// </summary>
        /// <returns>GUID for the device</returns>
        private string GetGUID()
        {
            string g;
            const string folderpath = @"../../config/";
            const string filename = "SourceGUID.key";
            const string fullpath = folderpath + filename;

            if (File.Exists(fullpath))
            {
                using (var sr = new StreamReader(fullpath, Encoding.UTF8))
                {
                    g = sr.ReadLine();
                    sr.Close();
                }
            }
            else
            {
                g = Guid.NewGuid().ToString();

                if (Directory.Exists(folderpath) == false)
                    Directory.CreateDirectory(folderpath);
                using (var fs = new FileStream(fullpath, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(g);
                    sw.Close();
                }
            }
            return g;
        }

        /// <summary>
        /// Method to get the devices own IP adress.
        /// Runs through all the devices IP's and selects one used for InterNetwork.
        /// </summary>
        /// <returns>IP Adress as a string</returns>
        private string GetOwnIp()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
    }
}
