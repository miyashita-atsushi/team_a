using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FericaReader
{
    static class DataBaseAccess
    {

        /// <summary>
        /// 履歴データ書き込み
        ///解析がすべて終了したリストを代入する
        /// </summary>
        /// <param name="historyList"></param>
        public static void WhiteUserHistorySql(List<ICCard> historyList)
        {
             try
            {
                using(var conn = new SQLiteConnection("Data Source=" + Properties.Settings.Default.UserDBFilePath))
                {
                    conn.Open();
                    using(SQLiteCommand command = conn.CreateCommand())
                    {
                        var sb = new StringBuilder();
                        //ファイルがない場合は作成する
                        sb.Append("CREATE TABLE IF NOT EXISTS UserHistory(");
                        sb.Append("date TEXT NOT NULL ,");
                        sb.Append("terminal TEXT NOT NULL ,");
                        sb.Append("process TEXT NOT NULL ,");
                        sb.Append("payment INTEGER ,");
                        sb.Append("deposit INTEGER ,");
                        sb.Append("balance INTEGER ,");
                        sb.Append("inStationName TEXT ,");
                        sb.Append("outStationName TEXT,");
                        sb.Append("idm TEXT,");
                        sb.Append("transactionID TEXT UNIQUE");
                        sb.Append(")");

                        command.CommandText = sb.ToString();
                        command.ExecuteNonQuery();

                    using(var dataset = new DataSet())
                    {
                        foreach(var suica in historyList) 
                        { 
                            String sql = string.Format(
                                "INSERT INTO UserHistory(date, terminal, process, payment, deposit, balance,inStationName, outStationName, idm, transactionID)" +
                                " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                suica.Date, suica.Terminal, suica.Process, suica.Payment, suica.Deposit,suica.Balance,
                                suica.InStationName, suica.OutStationName, suica.IDm, suica.TransactionID
                                );

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

        //月合計
        public static List<CsvCalcResults> MonthTotal(DateTime selectDatetime ,string selectProcess = "")
        {
            string selTime = String.Format("WHERE strftime('%Y', date) LIKE '{0}%' ",selectDatetime.Year);
            string selPro = String.Format("and process = '{0}' ",selectProcess);

            var resultList = new List<CsvCalcResults>();
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Properties.Settings.Default.UserDBFilePath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        //すべての金額合計
                        //string sql = "SELECT SUM(deposit), SUM(payment) FROM UserHistory";
                        var sb = new StringBuilder();

                        sb.Append("SELECT idm, strftime('%Y/%m', date), SUM(deposit), SUM(payment)");
                        sb.Append("FROM UserHistory ");
                        sb.Append(selTime);
                        if (!(selectProcess.Equals("")))
                        {
                            sb.Append(selPro);
                        }
                        sb.Append("GROUP BY idm , strftime('%Y/%m', date) ");
                        
                        command.CommandText = sb.ToString();
                        using (SQLiteDataReader sdr = command.ExecuteReader())
                        {
                            //月末を調べる
                            while (sdr.Read() == true)
                            {
                                var date = DateTime.Parse((sdr[1].ToString()+"/01"));
                                int days = DateTime.DaysInMonth(date.Year,date.Month);
                                    var res = new CsvCalcResults() {
                                        IDm = sdr[1].ToString(),
                                        FromDate = new DateTime(date.Year, date.Month, 1),
                                        ToDate = new DateTime(date.Year, date.Month, days),
                                        Deposit = sdr.GetInt32(2),
                                        Payment = sdr.GetInt32(3)
                                    };
                             
                                res.FromToDate = string.Format("{0}\r\n{1}",res.FromDate.ToString("yy/MM/dd"),res.ToDate.ToString("yy/MM/dd"));
                                resultList.Add(res);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return resultList;
        }

        public static List<CsvCalcResults> WeekTotal(DateTime selectDatetime,string selectProcess = "")
        {
            //SQLクエリ作成
            string selTime = String.Format("WHERE strftime('%Y/%m', date) LIKE '{0}%' ",selectDatetime.ToString("yyyy/MM"));
            string selPro = String.Format("and process = '{0}' ",selectProcess);
            var weeklyNumDic = WeeklyFormToDaysCalc(selectDatetime);
            var resultList = new List<CsvCalcResults>();
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Properties.Settings.Default.UserDBFilePath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        //すべての金額合計
                        //string sql = "SELECT SUM(deposit), SUM(payment) FROM UserHistory";
                        var sb = new StringBuilder();

                        sb.Append("SELECT strftime('%W', date) , idm, strftime('%Y/%m/%d', date) , SUM(deposit), SUM(payment)");
                        sb.Append("FROM UserHistory ");
                        sb.Append(selTime);
                        if (!(selectProcess.Equals("")))
                        {
                            sb.Append(selPro);
                        }
                        sb.Append("GROUP BY idm, strftime('%W', date) ");
                        
                        command.CommandText = sb.ToString();
                        using (SQLiteDataReader sdr = command.ExecuteReader())
                        {
                            //月末を調べる
                            int days = DateTime.DaysInMonth(selectDatetime.Year,selectDatetime.Month);
                            while (sdr.Read() == true)
                            {
                                var res = new CsvCalcResults() {};
                                    res.IDm = sdr[1].ToString();
                                    res.FromDate = weeklyNumDic[int.Parse(sdr[0].ToString())].First();
                                    res.ToDate = weeklyNumDic[int.Parse(sdr[0].ToString())].Last();
                                    res.Deposit = sdr.GetInt32(3);
                                    res.Payment = sdr.GetInt32(4);
                                    res.FromToDate = string.Format("{0}\r\n{1}",res.FromDate.ToString("yy/MM/dd"),res.ToDate.ToString("yy/MM/dd"));
                                resultList.Add(res);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return resultList;
        }
        
        /// <summary>
        /// CsvCalcResultで使用するFrom・ToDateに使用
        /// ※週を取得する際にSQLiteでは"00~52"で週を計算するのに対して
        /// 　こちらはGetWeekOfYearで取得する値が"1~53"なのでここで-1をすることでつじつまを合わせる
        /// </summary>
        /// <param name="selectDatetime"></param>
        /// <returns></returns>
        private static Dictionary<int, List<DateTime>> WeeklyFormToDaysCalc(DateTime selectDatetime)
        {
            var WeeklyNumberDic = new Dictionary<int, List<DateTime>>();
            var lastDays = DateTime.DaysInMonth(selectDatetime.Year,selectDatetime.Month);
            var searchMonth = new DateTime(selectDatetime.Year,selectDatetime.Month,1);
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            for(int i = 1;i <= lastDays ; i++)
            {
                var yearOfWeek = myCal.GetWeekOfYear(searchMonth,CalendarWeekRule.FirstDay, DayOfWeek.Monday) - 1;
                if(!(WeeklyNumberDic.ContainsKey(yearOfWeek)))
                {
                    WeeklyNumberDic.Add(yearOfWeek,new List<DateTime>(){searchMonth});
                }
                else
                {
                    WeeklyNumberDic[yearOfWeek].Add(searchMonth);
                }
                searchMonth = myCal.AddDays(searchMonth,1);
            }
            return WeeklyNumberDic;
        }
  

        //駅名検索
        public static string GetStationName(int areaCode,int lineCode,int stationCode)
        {
            string sql = 
                string.Format("SELECT StationName FROM StationDB WHERE AreaCode='{0}' AND LineCode='{1}' AND StationCode='{2}'",
                                  Convert.ToString(areaCode, 16), Convert.ToString(lineCode, 16), Convert.ToString(stationCode, 16));
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
    }
}
