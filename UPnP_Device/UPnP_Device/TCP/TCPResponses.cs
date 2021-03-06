using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;
using System.Xml.Linq;
using UPnP_Device.XML;

namespace UPnP_Device.TCP
{
    public class DescriptionReader
    {
        private string _path ;
        
        public DescriptionReader(string path)
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
        //private XMLWriter1 writer;
        private DescriptionReader XMLreader;

        public GETResponder(string path)
        {
            XMLreader = new DescriptionReader(path);
        }

        public void Respond(INetworkUtillity utillity)
        {
            string body = XMLreader.Read();
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
        }
        
        
    }

    public class POSTResponder: IRespondStrategy
    {
        private string message = "";
        private string action = "";
        private IOrder orderClass;

        public POSTResponder(string ctrl)
        {
            message = ctrl;
        }

        public void Respond(INetworkUtillity util)
        {
            action = DetermineOrder(message);
            var args = DetermineArgs(message,action);
            Console.WriteLine("\nPost action was: " + action);
            Console.WriteLine("With arguments: ");
            foreach (var arg in args)
            {
                Console.Write(arg + " , ");
            }
            
            orderClass = new Order(action, util);
            orderClass.execOrder(args);
        }

        //Determine the action type
        public string DetermineOrder(string ctl)
        {
            string[] splitter = new string[] {"\r\n"};
            string[] splitOrder = ctl.Split(splitter, StringSplitOptions.None);
            string soapLine = "";

            foreach (var s in splitOrder)
            {
                if (s.Contains("SOAPACTION"))
                {
                    soapLine = s;
                    break;
                }
            }

            string[] urn = soapLine.Split(':');
            string versionAndAction = urn.Last();
            string[] action = versionAndAction.Split('#')[1].Split('"');
            

            return action[0];
        }

        //Determine argument-list
        public List<UPnPArg> DetermineArgs(string wholeMessage, string actionName)
        {
            string[] splitter = new string[] { "\r\n\r\n" };
            string[] HeadAndBody = wholeMessage.Split(splitter, StringSplitOptions.None);
            
            XMLReader reader = new XMLReader();
            var args = reader.ReadArguments(HeadAndBody[1], actionName);

            return args;
        }
    }

    /// <summary>
    /// This class will take care of dealing with the order
    /// and respond appropriately over the TCP connection. 
    /// If TCP connection is closed prematurely by control point, 
    /// the senders address will still be in the mesage. (Handle this?)
    /// </summary>
    public interface IOrder
    {
        void execOrder(List<UPnPArg> argList);
    }
     
    public class Order : IOrder
    {
        private string action = "default";
        private InvokeResponseGen invokeResponseGen;
        private INetworkUtillity util;
        private System.Timers.Timer timer;
        
        public Order(string order, INetworkUtillity utillity)
        {
            action = order;
            invokeResponseGen = new InvokeResponseGen();
            util = utillity;

            timer = new System.Timers.Timer();
        }

        public void execOrder(List<UPnPArg> argList)
        {
            var args = new UPnPEventArgs(argList, action);
            CallBack cb = new CallBack(CallBackFunction);
            EventContainer.RaiseActionEvent(this, args, cb);

            timer.Elapsed += ConnectionTimedOut;
            timer.Interval = 30000; //UPnP Default timeout is 30 seconds
            timer.Enabled = true;
        }

        public void CallBackFunction(List<UPnPArg> argList, string act)
        {
            if (util.IsConnected())
            {
                string response = invokeResponseGen.InvokeResponse(act, argList);

                if(TCPDebug.DEBUG) 
                    Console.WriteLine("invoke answer: \n\r" + response);

                util.Send(response);
                util.Close();

                timer.Elapsed -= ConnectionTimedOut;
                timer.Enabled = false;
                timer.Dispose();
            }
        }

        private void ConnectionTimedOut(object e, ElapsedEventArgs args)
        {
            if (util.IsConnected())
            {
                string act = "TimedOut";
                List<UPnPArg> argList = new List<UPnPArg>();

                string response = invokeResponseGen.InvokeResponse(act, argList);

                Console.WriteLine("invoke answer: \n\r" + response);

                util.Send(response);
                util.Close();

                timer.Elapsed -= ConnectionTimedOut;
                timer.Enabled = false;
                timer.Dispose();
            }
            
        }
    }
     
 
}