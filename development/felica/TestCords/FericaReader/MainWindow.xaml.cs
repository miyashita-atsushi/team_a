using FelicaLib;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using System.Data;

namespace FericaReader
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
       public MainWindow()
        {
            InitializeComponent();

            Log4netManager.GetInstance().logger.Info("MainWindow　Start");

            //履歴DB作成・存在するか確認
            //IO.GetInstance().CreateUserDB();
            this.DataContext = this;

            //データ項目Processの種類ごとの集計を行う
            //コンボボックスに種類項目を挿入
            using (var conn1 = new SQLiteConnection("Data Source=" + Properties.Settings.Default.ProcessDBFilePath))
            {
                conn1.Open();
                using (DataContext con1 = new DataContext(conn1))
                {
                    var UserHistory = con1.GetTable<ProcessDBTable>();
                    //IQueryable<Suica> result = from x in UserHistory orderby x.Process select x;
                    IQueryable<string> result1 = (from x in UserHistory select x.ProcessName).Distinct();

                    var list = result1.ToList();
                    //一番上に空白を入れて全件検索が行えるようにする
                    list.Insert(0, "");

                    // コンボボックスに設定
                    this.ComboProcess.ItemsSource = list;
                    this.ComboProcess.SelectedIndex = 0;
                }
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(var felica = new Felica())
                {
                    //felica.Polling(0xFFFF);
                    //byte[] data = felica.IDm();
                    //var data =  ReadFelcia(felica);
                    ReadFelcia(felica);
                }
                LabelResult.Content = "Great";
                LabelResult.Background = Brushes.Blue;
                //ViewReadData();
                //本来は別ボタンなどで書き込むか選択させる
                WindowManager.GetInstance().WriteUserHistoryDB();
                MonthTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LabelResult.Content = "Nooon";
                LabelResult.Background = Brushes.Red;

            }
        }

          //指定した数のレコード作成
        private async void ButtonCreateTestDB_Click(object sender, RoutedEventArgs e)
        {
            //int createnum = int.Parse(this.TextBoxCreateNum.Text);
            //this.ButtonCreateTestDB.IsEnabled = false;

            //var task = await IO.GetInstance().CreateDBAsync(createnum);

            //TextBoxCalcValue.Text += "----------------------------" + "\r\n";
            //TextBoxCalcValue.Text += task;
            //TextBoxCalcValue.Text += "\r\n";
            //this.ButtonCreateTestDB.IsEnabled = true;
        }

        //DBからの読み込み
        //Linq試し
        private void ButtonViewDB_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new SQLiteConnection("Data Source = "+ Properties.Settings.Default.UserDBFilePath))
            {
                conn.Open();
                // コマンドの実行
               using (DataContext con = new DataContext(conn)){
                    //Whereで検索する単語(process)
                    String searchProcess = ComboProcess.SelectedItem.ToString();
                    // データを取得
                    Table<Suica> tblSuica = con.GetTable<Suica>();
    
                    IQueryable<Suica> result;
                    result =    from x in tblSuica
                                where x.Process.StartsWith(searchProcess)
                                select x;
                    
                    this.DataGridResult.ItemsSource = result.ToList();
                }
                // 切断
                conn.Close();
            }
        }

        //合計金額以外を表現、表示したい場合は新規クラス作成してListの要素としてそのクラスを所持する
        private void ButtonWeekly_Click(object sender, RoutedEventArgs e)
        {
            //子の変数に検索したい年月を代入
            DateTime YearMonth = new DateTime(2019,11,1);
            var list = WeeklyTotal(YearMonth);
            
            this.DataGridResult.ItemsSource = list.Select(k=> new {Value = k}).ToList();;
        }

        private void ButtonMakeChart_Click(object sender, RoutedEventArgs e)
        {
            this.SetChartModelValues();
        }

        //グラフ作成用クラス
        public Func<double, string> Formatter { get; set; } 
        public SeriesCollection seriesCollection { get; set; } 
        public class ResultDateModel 
        { 
             public double DateTime { get; set; } 
             public double Value { get; set; } 
        } 

        /// <summary>
        /// グラフ作成
        /// 現時点では出入金の合計を線グラフで引いているので
        /// Payment、Depositで分けたい場合はLineSeriesをもう一セット用意
        /// 下記コードは週集計だけ対応させた汎用性のないものなので
        /// 実装時には月集計、用途ごとなどにも変更を行えるようにする必要がある
        /// Formatterに関しては週
        /// </summary>
        public void SetChartModelValues()
        {
            var dayConfig = Mappers.Xy<ResultDateModel>() 
              .X(dateModel => dateModel.DateTime) 
              .Y(dateModel => dateModel.Value);
            var time = new DateTime(2019,11,1);
            var resultList = WeeklyTotal(time);
            //var resultList = new List<int>() {100,200,300,400,500 };
            this.seriesCollection = new SeriesCollection(dayConfig) 
            {
                new LineSeries() 
                { 
                    Values = new ChartValues<ResultDateModel>() 
                    {
                        new ResultDateModel(){DateTime = 1,Value = resultList[0]},
                        new ResultDateModel(){DateTime = 2,Value = resultList[1]},
                        new ResultDateModel(){DateTime = 3,Value = resultList[2]},
                        new ResultDateModel(){DateTime = 4,Value = resultList[3]},
                        new ResultDateModel(){DateTime = 5,Value = resultList[4]}
                    }
                } 
            };
            this.Formatter = value => value + "週目";
        }

        private void ReadFelcia(Felica felica)
        {
            felica.Polling(0xFFFF);
            byte[] data = felica.IDm();
            string dataStr = "";

            for(int i = 0; i < data.Length; i++)
            {
                dataStr += data[i].ToString("X2");
            }

            for (int i = 0; ; i++)
            {
                var history = felica.ReadWithoutEncryption(Suica.SERVICE_SUICA_HISTORY, i);
                if (history == null) break;
                WindowManager.GetInstance().AddHistryList(history,dataStr);
            }
            WindowManager.GetInstance().CalcuValue();
            this.DataGridResult.ItemsSource = WindowManager.GetInstance().GetHistoryList();
        }

        //月合計
        private void MonthTotal()
        {
            this.TextBoxCalcValue.Clear();
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

                        //月集計
                        sb.Append("SELECT strftime('%m月', date), SUM(deposit), SUM(payment) ");
                        sb.Append("FROM UserHistory ");
                        sb.Append("GROUP BY strftime('%Y/%m', date)");

                        //週、又は月集計
                        command.CommandText = sb.ToString();
                        //すべての金額集計を表示したい場合に使用
                        //command.CommandText = sql;
                        using (SQLiteDataReader sdr = command.ExecuteReader())
                        {
                            while (sdr.Read() == true)
                            {
                                ReadSingleRow((IDataRecord)sdr);
                                //すべての金額集計を表示したい場合に使用
                                //TextBoxCalcValue.Text += "入金: " + sdr.GetInt32(0).ToString() + "\r\n";
                                //TextBoxCalcValue.Text += "出金: " + sdr.GetInt32(1).ToString() + "\r\n";
                            }
                        }
                    }

                    conn.Close();
                }
                ////結果
                //var resultTotal = new List<int>();

                //using (var conn = new SQLiteConnection("Data Source = " + UserDBFilePath))
                //{
                //    conn.Open();
                //    //コマンドの実行
                //   using (DataContext con = new DataContext(conn))
                //    {
                //        //データテーブルを取得
                //        Table<Suica> tblSuica = con.GetTable<Suica>();

                //        ////集計する年月を決定
                //        //string seachKey = string.Format("{0}-{1}",searchMonth.Year,searchMonth.Month);
                //        //SQLiteの日付は、保存時は文字列
                //        var tesutResult = from x in tblSuica
                //                          //where x.Date.StartsWith(seachKey)
                //                          select x;

                //        foreach (var s in tesutResult.ToList())
                //        {
                //            DateTime testdate = DateTime.Parse(s.Date);
                //            int week = testdate.Day / 7;
                //            if(testdate.Day % 7 != 0)
                //            {
                //                ++week;
                //            }
                //            if(s.Deposit == 0 && 0 < s.Payment)
                //            {
                //                resultTotal[week - 1] -= s.Payment;
                //            }
                //            else if(s.Payment == 0 && 0 < s.Deposit)
                //            {
                //                resultTotal[week - 1] += s.Deposit;
                //            }
                //        }
                //    }
                //    //切断
                //    conn.Close();
                //}
            }
            catch (System.Exception e)
            {
                // ファイルが開かない場合
                System.Console.WriteLine(e.Message);
            }
        }
       /// <summary>
       /// SQLiteでは月での週集計はデフォルトで持っていないようなので
       /// 月初から7日ごとに金額を集計
       /// とりあえず出入金の合計を計算
       /// </summary>
       /// <param name="searchMonth"></param>
        private List<int> WeeklyTotal(DateTime searchMonth)
        {
            //結果
            var resultTotal = new List<int>(){0,0,0,0,0};

            using (var conn = new SQLiteConnection("Data Source = " + Properties.Settings.Default.UserDBFilePath))
            {
                conn.Open();
                //コマンドの実行
               using (DataContext con = new DataContext(conn))
                {
                    //データテーブルを取得
                    Table<Suica> tblSuica = con.GetTable<Suica>();

                    //集計する年月を決定
                    string seachKey = string.Format("{0}-{1}",searchMonth.Year,searchMonth.Month);

                    //SQLiteの日付は、保存時は文字列
                    var tesutResult = from x in tblSuica
                                      //where string.Format("{0}-{1}",x.Date.Year,x.Date.Month).StartsWith(seachKey)
                                      where x.Date.StartsWith(seachKey)
                                      select x;

                    foreach (var s in tesutResult.ToList())
                    {
                        DateTime testdate = DateTime.Parse(s.Date);
                        //DateTime testdate = s.Date;
                        int week = testdate.Day / 7;
                        if(testdate.Day % 7 != 0)
                        {
                            ++week;
                        }
                        if(s.Deposit == 0 && 0 < s.Payment)
                        {
                            resultTotal[week - 1] -= s.Payment;
                        }
                        else if(s.Payment == 0 && 0 < s.Deposit)
                        {
                            resultTotal[week - 1] += s.Deposit;
                        }
                    }
                }
                //切断
                conn.Close();

                return resultTotal;
            }
        }

        private void ReadSingleRow(IDataRecord record)
        {
            TextBoxCalcValue.Text += "----------------------------" + "\r\n";
            TextBoxCalcValue.Text += string.Format("{0} 入金: {1} 出金: {2}",record[0],record[1],record[2]);
            TextBoxCalcValue.Text += "\r\n";
        }

        private void WriteResult(Suica suica)
        {
            TextBoxCalcValue.Text += "----------------------------" + "\r\n";
            TextBoxCalcValue.Text += string.Format("{0} 入金: {1} 出金: {2}",suica.Date,suica.Deposit,suica.Payment);
            TextBoxCalcValue.Text += "\r\n";
        }

    }
}
