using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Containers;
using DBClasses;

namespace DatabaseLookupMain
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new DbLookup();

            var trkList = data.Browse("hej");

            foreach (ITrack t in trkList)
            {
                Console.WriteLine("---- " + t.FileName + "----");
                Console.WriteLine("Track Title: " + t.Title);
                Console.WriteLine("Track DeviceIP: " + t.DeviceIP);
                Console.WriteLine("Track Artist: " + t.Artist);
                Console.WriteLine("Track Album: " + t.Album);
                Console.WriteLine("Track Duration: " + t.Duration);
                Console.WriteLine("Track Genre: " + t.Genre);
                Console.WriteLine("Track Protocol: " + t.Protocol);
                Console.WriteLine("Track Path: " + t.Path);
                Console.WriteLine();
            }
            Console.ReadLine();

        }
    }
}
