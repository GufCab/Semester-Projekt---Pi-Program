using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UPnP_Device.XML
{
    class XMLServicesConfig
    {
        public List<FunctionProperties> _functions = new List<FunctionProperties>();

        //loads configfile and puts lines in list
        public void LoadConfig(string path)
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                List<string> lines = new List<string>();
                string line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                SeperateFunction(lines);
            }
        }

        //seperates functionname from the rest
        private void SeperateFunction(List<string> configInfo)
        {
            //List<FunctionProperties> functionProperties = new List<FunctionProperties>();
            int i = 0;

            foreach (string line in configInfo)
            {
                string[] function = line.Split('/');

                _functions.Add(new FunctionProperties());
                _functions[i].functionName = function[0];
                _functions[i].tmpString.Add(function[1]);
                
                ++i;
            }

            SeperateArguments();
        }

        //splits each argument
        private void SeperateArguments()
        {
            foreach (FunctionProperties functionPropertie in _functions)
            {
                string[] strings = functionPropertie.tmpString[0].Split(';');

                functionPropertie.tmpString.Clear();

                foreach (string argument in strings)
                {
                    functionPropertie.tmpString.Add(argument);
                }
            }
            
            SeperateArgumentInternal();  
        }

        //splits and sets argumentName, Direction and relatedStateVariable
        private void SeperateArgumentInternal()
        {
            int i = 0;

            foreach (FunctionProperties functionPropertie in _functions)
            {
                foreach (string s in functionPropertie.tmpString)
                {
                    functionPropertie.arguments.Add(new ArgumentProperties());
                    
                    string[] s2 = s.Split(':');
                    functionPropertie.arguments[i].argumentName = s2[0];
                    functionPropertie.arguments[i].direction = s2[1];
                    functionPropertie.arguments[i].relatedStateVariable = s2[2];
                    functionPropertie.arguments[i].sendEventAttribute = s2[3];
                    functionPropertie.arguments[i].dataType = s2[4];

                    ++i;
                }

                functionPropertie.tmpString.Clear();

                i = 0;
            }
        }
    }

    public class FunctionProperties
    {
        public FunctionProperties()
        {
            arguments = new List<ArgumentProperties>();
            tmpString = new List<string>();
        }

        public string functionName { get; set; }
        public List<ArgumentProperties> arguments;

        //only used under contruction of ArgumentProperties
        public List<string> tmpString { set; get; }
    }

    public class ArgumentProperties
    {
        public string argumentName { get; set; }
        public string direction { get; set; }
        public string relatedStateVariable { get; set; }
        public string sendEventAttribute { get; set; }
        public string dataType { set; get; }
    }

}
