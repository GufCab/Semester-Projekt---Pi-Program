using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace UPnP_Device.TCP
{
    public class XMLReader
    {
        private string _path ;
        
        public XMLReader(string path)
        {
            _path = @"./Descriptions" + path + "desc.xml";
        }
        
        public string Read()
        {
            string s = "-1";
           

            if (File.Exists(_path))
            {
                using (var sr = new StreamReader(_path, Encoding.UTF8))
                {
                    s = "";
                    s = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return s;
        }
    }

    public class GETResponder : IRespondStrategy
    {
        private XMLWriter1 writer;
        private XMLReader XMLreader;

        public GETResponder(string path)
        {
            XMLreader = new XMLReader(path);
        }

        public void Respond(INetworkUtillity utillity)
        {
            string body = XMLreader.Read();
        
            //Todo: Device is currently not showing up in Device Spy, because it can't find a proper Service Description.
            //Todo: Implement one!

            string head = "HTTP/1.1 200 OK\r\n" +
                          "CONTENT-TYPE: text/xml;charset=utf-8\r\n" +
                          "CONTENT-LENGTH: " + body.Length + "\r\n";
            
            string message = head + "\r\n" + body;

            Console.WriteLine("TCP Message: \r\n" + message);

            try
            {
                utillity.Send(message);
            }
            catch (Exception)
            {
                Console.WriteLine("Exception in Reponse");
                throw;
            }

            //Console.WriteLine("rec: " + utillity.Receive());

            //Todo: What is happening with TCP connection
        }
        
        
    }

    public class POSTResponder: IRespondStrategy
    {
        public void Respond(INetworkUtillity util)
        {
            //Todo: implementation of POST requests
        }
    }


}
