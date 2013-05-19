using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Containers;
using MySql.Data.MySqlClient;

namespace DBClasses
{
    public class DBLookup
    {
        private string _conStr = "server=127.0.0.1;userid=PiLocal;password=pilocal;database=Piindex";
        //private MySqlConnection _con;


        public List<ITrack> Browse(string info)
        {
            //MySqlCommand c = new MySqlCommand("Hej");

            MySqlConnection _con;

            try
            {
                _con = new MySqlConnection(_conStr);
                _con.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("DB Connetion error");
                throw;
            }


            /*
            var t = new List<ITrack>();

            Console.WriteLine("In dblookup handler");

            t.Add(new Track());
            return t;
            */
            
            List<ITrack> trkList;

            //SetupConnection();

            string stm = "SELECT * FROM PIMusikData " +
                         "INNER JOIN PIFilePath ON PIMusikData.FilePath_UUIDPath = PIFilePath.UUIDPath " +
                         "INNER JOIN PIDevice ON PIFilePath.Device_UUIDDevice = PIDevice.UUIDDevice";

            MySqlCommand cmd = new MySqlCommand(stm, _con);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                trkList = new List<ITrack>();

                while (reader.Read())
                {
                    var trk = new Track();

                    trk.Title = reader.GetString(1);
                    trk.Duration = reader.GetUInt32(2).ToString();
                    trk.FileName = reader.GetString(3);
                    trk.Artist = reader.GetString(4);
                    trk.Album = reader.GetString(5);
                    trk.Genre = reader.GetString(6);

                    trk.Path = reader.GetString(9);
                    trk.DeviceIP = reader.GetString(12);
                    trk.Protocol = reader.GetString(14);

                    trkList.Add(trk);
                }
            }

            if (_con != null)
                _con.Close();

            return trkList;
            
        }
        /*
        private void SetupConnection()
        {

            try
            {
                _con = new MySqlConnection(_conStr);
                _con.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("DB Connetion error");
                throw;
            }
        }

    }*/

            /*
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

                    trk.FileName = elm.trk.FileName + ".mp3";
                    trk.Title = elm.trk.Title;
                    //trk.Duration = (elm.trk.NrLenth);
                    trk.Duration = elm.trk.NrLenth.ToString();
                    trk.Artist = elm.trk.Artist_Artist;
                    trk.Album = elm.trk.Album_Album;
                    trk.Genre = elm.trk.Genre_Genre;

                    trackList.Add(trk);
                }
            }
            
            return trackList;
        }
    }
     */
    }
}
