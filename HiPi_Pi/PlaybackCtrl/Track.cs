using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Playback
{
    public interface ITrack 
    {
        string DeviceIP { get; set; }
        string Path { get; set; }

        string Title { get; set; }
        int Duration { get; set; }
        string Artist { get; set; }
        string Album { get; set; }
        string Genre { get; set; }
    }

    public class Track : ITrack
    {
        public string DeviceIP { get; set; }
        public string Path { get; set; }

        public string Title { get; set; }
        public int Duration { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
    }
}
