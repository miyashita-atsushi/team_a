using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadPasori
{
    class IO
    {
        private static IO Instance = new IO();
        public String UserDBFilePath = @"DB\UserHistory.sqlite";
        public String UserCSVFilePath = @"DB\UserHistory.csv";

        public static IO GetInstance()
        {
            return Instance;
        } 

        public class SuicaMapper : CsvHelper.Configuration.ClassMap<Suica>
        {
            public SuicaMapper()
            {
                Map(x => x.Date          ).Index(0).Name("日付");
                Map(x => x.Terminal      ).Index(1).Name("端末");
                Map(x => x.Process       ).Index(2).Name("処理");
                Map(x => x.Payment       ).Index(3).Name("入金");
                Map(x => x.Deposit       ).Index(4).Name("出金");
                Map(x => x.InStationName ).Index(5).Name("入駅");
                Map(x => x.OutStationName).Index(6).Name("出駅");
                Map(x => x.Balance       ).Index(7).Name("残高");
                Map(x => x.ByteData      ).Index(8).Name("取得データ");
                //Map(x => x.).Index(9).Name("連番");
                //Map(x => x.).Index(10).Name("地域コード");
                //Map(x => x.).Index(11).Name("Binary");
            }
        }

        //private void WriteCsv(List<Suica> suica, string filePath)
        public void WriteCsv(List<Suica> suica)
        { 
            try
            {
                using (var sw = new StreamWriter(UserCSVFilePath))
                using (var csv = new CsvWriter(sw,CultureInfo.CurrentCulture))
                {
                    List<Suica> writeData = suica;
                    //writeData.Add(suica);
                    //writeData.Add(
                    //    new Suica() { 
                    //    Date          = suica.Date,          
                    //    Process       = suica.Process       ,
                    //    Terminal      = suica.Terminal      ,
                    //    GetStation    = suica.GetStation    ,
                    //    InStationArea = suica.InStationArea ,
                    //    InStationName = suica.InStationName ,
                    //    OutStationArea= suica.OutStationArea,
                    //    OutStationName= suica.OutStationName,
                    //    Balance       = suica.Balance       ,
                    //    Seq           = suica.Seq           ,
                    //    Region        = suica.Region        ,
                    //    ByteData      = suica.ByteData      });
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.RegisterClassMap<SuicaMapper>();

                    csv.WriteRecords(writeData);
     
                }
                //ファイルが存在する
                //if (File.Exists(filePath))
                //{
                //}
                //////ファイルが存在しない
                //else
                //{
                //    //using (var sw = new StreamWriter(filePath, false))
                //    //{

                //    //}
                //}

            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                System.Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 履歴データ書き込み
        ///解析がすべて終了したリストを代入する
        /// </summary>
        /// <param name="historyList"></param>

        public void WhiteUserHistorySql(List<Suica> historyList)
        {
             try
            {
                using(var conn = new SQLiteConnection ("Data Source=" + UserDBFilePath))
                {
                    conn.Open();
                    using(SQLiteCommand command = conn.CreateCommand())
                    {
                        var sb = new StringBuilder();
                        //ファイルがない場合は作成する
                        sb.Append("CREATE TABLE IF NOT EXISTS UserHistory(");
                        sb.Append("date TEXT NOT NULL ,");
                        sb.Append("type TEXT NOT NULL ,");
                        sb.Append("payment INTEGER ,");
                        sb.Append("deposit INTEGER ,");
                        sb.Append("getonStation TEXT ,");
                        sb.Append("getoffStation TEXT");
                        sb.Append(")");

                        command.CommandText = sb.ToString();
                        command.ExecuteNonQuery();

                    using(var dataset = new DataSet())
                    {
                        foreach(var suica in historyList) 
                        { 
                            String sql = string.Format(
                                "INSERT INTO UserHistory(date, type, payment, deposit, getonStation, getoffStation)" +
                                " VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                suica.Date, suica.ProcessCode, suica.Payment, suica.Deposit,suica.InStationName,suica.OutStationName);

                            var dataAdapter = new SQLiteDataAdapter(sql,conn);
                            dataAdapter.Fill(dataset);}
                        }
                    }
                    conn.Close();
                }

            }
            catch (System.Exception e)
            {
                // ファイルが開かない場合
                System.Console.WriteLine(e.Message);
            }
        }


    }
}
