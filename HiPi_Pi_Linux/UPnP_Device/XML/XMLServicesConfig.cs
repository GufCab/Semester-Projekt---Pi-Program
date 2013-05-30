using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UPnP_Device.XML
{
    /// <summary>
    /// Reads a configfile and creates a service description from that file
    /// </summary>
    public class XMLServicesConfig
    {
        public List<FunctionProperties> _functions = new List<FunctionProperties>();
        public string deviceType;

        /// <summary>
        /// Creates a service description for each string in hte list
        /// </summary>
        /// <param name="paths">A list of paths of configfiles</param>
        /// <param name="writer">The XMLWriter object that creates the service descriptions</param>
        public XMLServicesConfig(List<string> paths, IXMLWriter writer)
        {
            foreach (string path in paths)
            {
                LoadConfig(path);
                writer.GenServiceDescription(deviceType, _functions);
                _functions.Clear();
            }
        }
        
        /// <summary>
        /// Loads configfile and puts lines in list
        /// </summary>
        /// <param name="path"></param>
        private void LoadConfig(string path)
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                List<string> lines = new List<string>();
                string line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                deviceType = lines[0];
                lines.RemoveAt(0);

                SeperateFunction(lines);
            }
        }
        
        /// <summary>
        /// Seperates functionname from the restseperates functionname from the rest
        /// </summary>
        /// <param name="configInfo"></param>
        private void SeperateFunction(List<string> configInfo)
        {
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

        /// <summary>
        /// Splits each argument
        /// </summary>
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

        /// <summary>
        /// Splits and sets argumentName, Direction and relatedStateVariable
        /// </summary>
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

    /// <summary>
    /// Container that holds an argumentname and a list of properties
    /// </summary>
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

    /// <summary>
    /// Container that holds the properties
    /// </summary>
    public class ArgumentProperties
    {
        public string argumentName { get; set; }
        public string direction { get; set; }
        public string relatedStateVariable { get; set; }
        public string sendEventAttribute { get; set; }
        public string dataType { set; get; }
    }
}
