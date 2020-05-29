using FelicaLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FericaReader
{
    class WindowManager : INotifyPropertyChanged
    {
        public static WindowManager Instance = new WindowManager();

        public List<ICCard> ReadHistoryList = new List<ICCard>();
        public GraphView ResultsGraphView { get;set; } = new GraphView();

        //private List<CsvCalcResults> _ResultList = new List<CsvCalcResults>();

        //public List<CsvCalcResults> ResultList { 
        //    get{return  _ResultList;}
        //    set{
        //            _ResultList = value;
        //            OnPropertyChanged("ResultList");
        //       }

        public List<CsvCalcResults> ResultList {get;set; }
        private ObservableCollection<CsvCalcResults> _ResultItemsSource = new ObservableCollection<CsvCalcResults>();
        public ObservableCollection<CsvCalcResults> ResultItemsSource 
        {
            get {return _ResultItemsSource;}
            set {   _ResultItemsSource = value;
                    OnPropertyChanged("ResultItemsSource");
                }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private WindowManager()
        {

        }

        public void ReadFelcia(string iCCardType)
        {
            using (Felica felica = new Felica()) { 
                switch (iCCardType)
                {
                    case "Suica":
                        ReadSuica(felica);
                        break;
                    default :
                        break;
                }
            }
          
        }
        private void ReadSuica(Felica felica)
        {
            //読込む前にList初期化
            this.ReadHistoryList = new List<ICCard>();
            felica.Polling(0xFFFF);
            byte[] data = felica.IDm();
            string idm = "";

            for(int i = 0; i < data.Length; i++)
            {
                idm += data[i].ToString("X2");
            }

            for (int i = 0; ; i++)
            {
                var history = felica.ReadWithoutEncryption(Suica.SERVICE_SUICA_HISTORY, i);
                if (history == null) break;

                AddHistryList(history,idm,new Suica());
            }
            CalcuValue();
            WriteUserHistoryDB();
        }

        private void WriteUserHistoryDB()
        {
            //リスト反転
            //this.HistoryList.Reverse();
            //CalcuValue();
            DataBaseAccess.WhiteUserHistorySql(this.ReadHistoryList);
            CsvWriter.WriteResultCsv(this.ReadHistoryList);
        }

        public List<string> SearchProcesses()
        {
            //データ項目Processの種類ごとの集計を行う
            //コンボボックスに種類項目を挿入
            using (var conn1 = new SQLiteConnection("Data Source=" + Properties.Settings.Default.UserDBFilePath))
            {
                conn1.Open();
                using (DataContext con1 = new DataContext(conn1))
                {
                    var UserHistory = con1.GetTable<ICCard>();
                    IQueryable<string> ProcessResult = (from x in UserHistory select x.Process).Distinct();
                    //IQueryable<SerchData> ProcessResult = (from x in UserHistory
                    //                                       select new SerchData { Date = x.Date, Process = x.Process }).Distinct();

                    var list = ProcessResult.ToList();
                    //一番上に空白を入れて全件検索が行えるようにする
                    list.Insert(0, "");
                    return list;
                }
            }
        }

        /// <summary>
        /// 読込んだデータ解析
        /// ■ICカードの種類は引数iccardで決める
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idm"></param>
        private void AddHistryList(byte[] data ,string idm,ICCard iccard)
        {
            var card = iccard;
            card.IDm = idm;
            card.AnalyzeTransaction(data);
            ReadHistoryList.Add(card);
        }

        /// <summary>
        /// Suicaは出入金自体の値を保存していないので最初に読み込んだ残額から
        /// 現在の残額を計算しなければならない
        /// そのため最古の履歴は計算できないので削除し、残高だけを利用する
        /// </summary>
        /// <returns></returns>
        private void CalcuValue()
        {
            int prevBalance = 0;
            this.ReadHistoryList.Reverse();

            foreach (ICCard t in this.ReadHistoryList)
            {
                t.Value = t.Balance - prevBalance;
                prevBalance = t.Balance;
            }
            ReadHistoryList.RemoveAt(0);   // 最古の履歴は捨てる
        }

        public void AggregateResults(bool MonthorWeek,DateTime selectTime, string selectpro = "")
        {
            //週集計
            if (MonthorWeek == true)
            {
                //if (selectpro.Equals(""))
                //{
                //    this.ResultList = DataBaseAccess.WeekTotal(selectTime);
                //}
                //else
                //{
                    this.ResultList = DataBaseAccess.WeekTotal(selectTime, selectpro);
                //}
            }
            //月集計
            else if (MonthorWeek == false)
            {
                //if (selectpro.Equals(""))
                //{
                //    this.ResultList= DataBaseAccess.MonthTotal(selectTime);
                //}
                //else
                //{
                    this.ResultList = DataBaseAccess.MonthTotal(selectTime, selectpro);
                //}
            }

            if (ResultList != null && ResultList.Count != 0)
            {

                //DataGridDBResult.ItemsSource = WindowManager.Instance.ResultList;
                this.ResultItemsSource = new ObservableCollection<CsvCalcResults>(this.ResultList);
                ViewResultGraph();
            }
        }
        private void ViewResultGraph()
        {
            ResultsGraphView.X.Reset();
            ResultsGraphView.Y.Reset();
            ResultsGraphView.Model.ResetAllAxes();
            ResultsGraphView.Model.Series.Clear();
            ResultsGraphView.Model.Axes.Clear();
            ResultsGraphView.Model.InvalidatePlot(true);
            ResultsGraphView.CreateGraph(ResultList);
        }



        ////DBからの読み込み
        ////Linq試用
        //private void ButtonViewDB_Click(object sender, RoutedEventArgs e)
        //{
        //    using (var conn = new SQLiteConnection("Data Source = "+ Properties.Settings.Default.UserDBFilePath))
        //    {
        //        conn.Open();
        //        // コマンドの実行
        //       using (DataContext con = new DataContext(conn)){
        //            //Whereで検索する単語(process)
        //            String searchProcess = ComboSelectProcess.SelectedItem.ToString();
        //            // データを取得
        //            Table<ICCard> tblSuica = con.GetTable<ICCard>();
    
        //            IQueryable<ICCard> result;
        //            result =    from x in tblSuica
        //                        where x.Process.StartsWith(searchProcess)
        //                        select x;
                    
        //            this.DataGridDBResult.ItemsSource = result.ToList();
        //        }
        //        // 切断
        //        conn.Close();
        //    }
        //}
    }
}
