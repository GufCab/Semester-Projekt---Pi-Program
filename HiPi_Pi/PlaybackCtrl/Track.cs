using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaybackCtrl
{
    public interface ITrack
    {
        
    }

    public class Track : ITrack
    {

        string URI { get; set; }

        public string deviceIP;

        public string Name;
        public int Duration;
        public string path;



    }
}
