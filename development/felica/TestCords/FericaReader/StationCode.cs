
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Text;

namespace FericaReader
{
    class StationCode : IDisposable
    {
        public StationCode()
        {
           
        }

        public void Dispose()
        {
        }
        //DBへのクエリ実行
        private string DoQuery(string sql)
        {
            string Result = null;
            using(var conn = new SQLiteConnection("Data Source =" + Properties.Settings.Default.StationDBFilePath))
            {
                conn.Open();
                using(SQLiteCommand command = conn.CreateCommand())
                {
                    var sb = new StringBuilder();
                    sb.Append(sql);
                    command.CommandText = sb.ToString();
                    using(SQLiteDataReader sdr = command.ExecuteReader())
                    {
                        if(sdr.Read() == true)
                        {
                            Result = sdr.GetString(0);
                        }
                    }
                }
                conn.Close();
            }
            return Result;
        }
        //クエリ作成
        public string GetStationName(int areaCode,int lineCode,int stationCode)
        {
            string sql = 
                string.Format("SELECT StationName FROM StationDB WHERE AreaCode='{0}' AND LineCode='{1}' AND StationCode='{2}'",
                                  Convert.ToString(areaCode, 16), Convert.ToString(lineCode, 16), Convert.ToString(stationCode, 16));
            return DoQuery(sql);
        }
    }
}
