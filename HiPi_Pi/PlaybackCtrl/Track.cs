using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Playback
{
    public interface ITrack 
    {
        string DeviceIP { get; set; }

        string Name { get; set; }
        int Duration { get; set; }
        string Path { get; set; }
    }

    public class Track : ITrack
    {
        public string DeviceIP { get; set; }

        public string Name { get; set; }
        public int Duration { get; set; }
        public string Path { get; set; }
    }
}
