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
        private string message = "";
        private string action = "";
        private IOrder orderClass;
        private OrderFactory orderFactory = new OrderFactory();

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

            orderClass = orderFactory.GetOrder(action);

            orderClass.execOrder(util, args);
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
            
            Console.WriteLine(soapLine);

            string[] urn = soapLine.Split(':');
            string versionAndAction = urn.Last();
            string[] action = versionAndAction.Split('#')[1].Split('"');
            

            return action[0];
        }

        //Determine argument-list
        public List<Tuple<string,string>> DetermineArgs(string wholeMessage, string actionName)
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
        void execOrder(INetworkUtillity utillity, List<Tuple<string,string>> argList);
    }
     
    public class PlayOrder : IOrder
    {
        private string action = "Play";

        public void execOrder(INetworkUtillity utillity, List<Tuple<string,string>> argList)
        {
            var invokeResponseGen = new InvokeResponseGen();

            string response = invokeResponseGen.InvokeResponse(action, argList);

            Console.WriteLine("invoke answer: \n\r" + response);
            utillity.Send(response);

            EventContainer.RaisePlayEvent(this, null);
            
            utillity.Close();
        }
    }

    public class StopOrder : IOrder
    {
        const string action = "Stop";

        public void execOrder(INetworkUtillity utillity, List<Tuple<string, string>> argList)
        {
            EventContainer.RaiseStopEvent(this, null);

            var invokeResponseGen = new InvokeResponseGen();

            string response = invokeResponseGen.InvokeResponse(action, argList);

            Console.WriteLine("invoke answer: \n\r" + response);
            utillity.Send(response);
            utillity.Close();

            EventContainer.RaiseStopEvent(this, null);
        }
    }

    public class PauseOrder : IOrder
    {
        private const string action = "Pause";

        
        public void execOrder(INetworkUtillity utillity, List<Tuple<string, string>> argList)
        {
            var invokeResponseGen = new InvokeResponseGen();

            string response = invokeResponseGen.InvokeResponse(action, argList);

            Console.WriteLine("invoke answer: \n\r" + response);
            utillity.Send(response);
            utillity.Close();

            EventContainer.RaisePauseEvent(this, null);
        }

        
    }

    public class PreviousOrder : IOrder
    {
        private const string action = "Previous";


        public void execOrder(INetworkUtillity utillity, List<Tuple<string, string>> argList)
        {
            var invokeResponseGen = new InvokeResponseGen();

            string response = invokeResponseGen.InvokeResponse(action, argList);

            Console.WriteLine("invoke answer: \n\r" + response);
            utillity.Send(response);
            utillity.Close();

            EventContainer.RaisePreviousEvent(this, null);
        }
    }
    public class SetTransportURIOrder : IOrder
    {
        private const string action = "Next";


        public void execOrder(INetworkUtillity utillity, List<Tuple<string, string>> argList)
        {
            var invokeResponseGen = new InvokeResponseGen();

            string response = invokeResponseGen.InvokeResponse(action, argList);

            Console.WriteLine("invoke answer: \n\r" + response);
            utillity.Send(response);
            utillity.Close();

            foreach (Tuple<string, string> tuple in argList)
            {
                Console.WriteLine("Argument: " + tuple.Item1 + " Has value: " + tuple.Item2);
            }

            EventContainer.RaiseSetAVTransportURIEvent(this, null);
        }
    }
    public class NextOrder : IOrder
    {
        private const string action = "Next";


        public void execOrder(INetworkUtillity utillity, List<Tuple<string, string>> argList)
        {
            var invokeResponseGen = new InvokeResponseGen();

            string response = invokeResponseGen.InvokeResponse(action, argList);

            Console.WriteLine("invoke answer: \n\r" + response);
            utillity.Send(response);
            utillity.Close();

            EventContainer.RaiseNextEvent(this, null);
        }
    }

    public class OrderFactory
    {
        private Dictionary<string, IOrder> _strat= new Dictionary<string,IOrder>(); 

        public OrderFactory()
        {
            _strat.Add("Play", new PlayOrder());
            _strat.Add("Stop", new StopOrder());
            _strat.Add("Next", new NextOrder());
            _strat.Add("Pause", new PauseOrder());
            _strat.Add("Previous", new PreviousOrder());
            _strat.Add("SetAVTransportURI", new SetTransportURIOrder());
        }

        public IOrder GetOrder(string ord)
        {
            
            return _strat[ord];
        }

        
    } 
}