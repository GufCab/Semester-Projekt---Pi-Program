using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The Containers namespace includes different containers, that is used throughout the whole system as collections of data.
/// </summary>
namespace Containers
{

    /// <summary>
    /// Interface to container of track info
    /// </summary>
    public interface ITrack
    {
        string Path { get; set; }       //mappestruktur ifht live555
        string DeviceIP { get; set; }
        string FileName { get; set; }   //ex Jump.mp3
        string Protocol { get; set; }   //ex rtsp://

        string Title { get; set; }
        string Duration { get; set; }
        string Artist { get; set; }
        string Album { get; set; }
        string Genre { get; set; }
		string ParentID { get; set; }
    }

    /// <summary>
    /// Container of track info
    /// </summary>
    public class Track: ITrack
    {
        public string DeviceIP { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Protocol { get; set; }

        public string Title { get; set; }
        public string Duration { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
		public string ParentID { get; set; }

		public Track ()
		{
			DeviceIP = "";
			Path = "";
			FileName = "";
			Protocol = "";

			Title = "";
			Duration = "";
			Artist = "";
			Album = "";
			Genre = "";
			ParentID = "0";
		}
    }
}
