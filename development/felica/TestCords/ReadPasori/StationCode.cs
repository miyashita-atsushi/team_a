/*
 * FeliCa2Money
 *
 * Copyright (C) 2001-2011 Takuya Murakami
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 */

// stationcode.mdb アクセスクラス

using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Text;

namespace ReadPasori
{
    class StationCode : IDisposable
    {
        public string DBFilePath = @"DB\StationDB.sqlite";

        public StationCode()
        {
           
        }

        public void Dispose()
        {
        }

        //とりあえずpublicで本番はprivateでSQLはかけないように
        //別メソッド作成して値だけ持ってくるようにする
        public string doQuery(string sql)
        {
            string Result = null;
            using(var conn = new SQLiteConnection("Data Source =" + DBFilePath))
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

        public string GetStationName(int areaCode,int lineCode,int stationCode)
        {
            string sql = 
                string.Format("SELECT StationName FROM StationDB WHERE AreaCode='{0}' AND LineCode='{1}' AND StationCode='{2}'",
                                  Convert.ToString(areaCode, 16), Convert.ToString(lineCode, 16), Convert.ToString(stationCode, 16));
            //areaCode,lineCode,stationCode);
            return doQuery(sql);
        }
    }
}
