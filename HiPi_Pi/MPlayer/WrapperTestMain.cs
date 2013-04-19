using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPlayer
{
    public class WrapperTestMain
    {
        static void Main()
        {
            var testWrap = new Wrapper();

            testWrap.PlayTrack("LaGrange.mp3");
        }
    }
}
