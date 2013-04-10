using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiProgram
{
    public class WrapperMOut
    {
        private StreamReader _output;
        
        public WrapperMOut()
        {
            _output = null;
        }

        public void SetOutStream(StreamReader stream)
        {
            if (stream != null)
            {
                _output = stream;
            }                   
        }


    }
}
