using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiProgram
{
    public class WrapperTestMain
    {
        public void Main()
        {
            var testWrap = new Wrapper("LaGrange.mp3");

            testWrap.PlayTrack("LaGrange.mp3");
        }
    }
}
