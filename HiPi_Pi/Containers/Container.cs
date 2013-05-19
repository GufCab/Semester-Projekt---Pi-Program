using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Containers
{
    public class Container
    {
        public string id { get; set; }
        public string parentID { get; set; }
        public string title { get; set; }
        public string upnpClass { get; set; }
    }
}
