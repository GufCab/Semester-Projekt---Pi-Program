using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace UPnP_Device.TCP
{
    public static class DeviceInfo
    {
        public static string AV()
        {
            return "1.0";
        }

        public static string Cn()
        {
            return "Gruppe 8";
        }

        public static string Mn()
        {
            return "HiPi";
        }

        public static string Mv()
        {
            return "1.00";
        }
    }

    public class GetResponse : IRespondStrategy
    {
        private XMLWriter1 writer;

        public GetResponse()
        {
            writer = new XMLWriter1();
        }

        public void Respond(TCPUtillity utillity)
        {
            string body = writer.genGETxml();

            string head = "HTTP/1.1 200 OK\r\n" +
                          "CONTENT-TYPE: text/xml;charset=utf-8\r\n" +
                          "X-AV-Server-Info: av=\"" + IPHandler.GetInstance().AV + "\"; cn=" +
                          IPHandler.GetInstance().CN + "; mn=" + IPHandler.GetInstance().MN +
                          "; mv=" + IPHandler.GetInstance().MV + "\r\n" +
                          "X-AV-Physical-Unit-Info: pa=" + IPHandler.GetInstance().PA + "\r\n" +
                          "CONTENT-LENGTH: " + body.Length + "\r\n"+"\r\n";

            
            string message = head + body;

            Console.WriteLine("TCP Message: \r\n" + message);
            try
            {
                Console.WriteLine("Util ready to send");
                utillity.TCPSend(message);
                Console.WriteLine("Util send");
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}