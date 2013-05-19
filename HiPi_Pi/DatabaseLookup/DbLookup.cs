using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Containers;
using DatabaseLookup;

namespace DBClasses
{
    public class DbLookup
    {
        
        public List<ITrack> Browse(string info)
        {
            string[] lastInfo = info.Split('/');

            List<ITrack> trackList = new List<ITrack>();         

            using (var music = new PiindexEntities())
            {  
                //From each trk in DB, select (whole) trk
                var table = (from trk in music.PIMusikData
                         join p in music.PIFilePath on trk.FilePath_UUIDPath equals p.UUIDPath
                         join d in music.PIDevice on p.Device_UUIDDevice equals d.UUIDDevice
                         select new {trk, p, d}).ToList();
                
                foreach (var elm in table)
                {
                    ITrack trk = new Track();
                    trk.Path = elm.p.FilePath;
                    trk.DeviceIP = elm.d.IP;
                    trk.Protocol = elm.d.Protocol;

                    trk.Title = elm.trk.Title;
                    trk.Duration = Convert.ToInt32((elm.trk.NrLenth));
                    trk.Artist = elm.trk.Artist_Artist;
                    trk.Album = elm.trk.Album_Album;
                    trk.Genre = elm.trk.Genre_Genre;

                    trackList.Add(trk);
                }
            }
            
            return trackList;
        }

        private string GetFilePath(string path)
        {
            string str;
            using (var music = new PiindexEntities())
            {
                str = (from p in music.PIFilePath where p.UUIDPath == path select p.FilePath).ToString();
            }

            return str;
        }
    }
}
