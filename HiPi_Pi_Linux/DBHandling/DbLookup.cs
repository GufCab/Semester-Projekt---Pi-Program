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
		//private string _conStr = "server=192.168.1.100;userid=Hipi;password=pi;database=Piindex";
        private MySqlConnection _con;


        public List<ITrack> Browse(string info)
        {            
            List<ITrack> trkList;

            SetupConnection();

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
					trk.ParentID = "all";

                    trkList.Add(trk);
                }
            }

            if (_con != null)
                _con.Close();

            return trkList;
            
        }
        
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
    }
}
