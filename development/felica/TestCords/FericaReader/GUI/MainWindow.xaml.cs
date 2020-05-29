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
using System.Data;
using System.IO;
using System.Globalization;
using log4net;

namespace FericaReader
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool ResultPeriod = true;
        //private List<CsvCalcResults> ResultList;
        public static Log4netManager log = new Log4netManager() ;


        public MainWindow()
        {
            InitializeComponent();
            log.logger.Info("MainWindow　Start");
            this.DataContext = WindowManager.Instance;
            //とりあえずSuicaだけ
            this.ComboICCardType.Items.Add("Suica");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            testDB();
        }
        void testDB()
        {
            List<ICCard> ReadHistoryList = new List<ICCard>();
            var date = "2020-04-12";
            for(int i = 0 ;i < 15 ; i++)
            {
                var s = new Suica()
                {
                    Date = date,
                    Terminal="出金",
                    Process="出金",
                    Payment = 500,
                    Deposit = 0,
                    Balance =0,
                    InStationName="",
                    OutStationName="",
                    IDm="TestIDM",

                    TransactionID=DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"+i)
                };
                ReadHistoryList.Add(s);
                s = new Suica()
                {
                    Date = date,
                    Terminal="入金",
                    Process="入金",
                    Payment = 0,
                    Deposit = 100,
                    Balance =0,
                    InStationName="",
                    OutStationName="",
                    IDm="TestIDM",
                    TransactionID=DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"+i)
                };
                ReadHistoryList.Add(s);
            }
            DataBaseAccess.WhiteUserHistorySql(ReadHistoryList);
        }

        //カード種別を選んでからクリックさせる
        private void ButtonReadFelica_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if( this.ComboICCardType.SelectedItem == null || this.ComboICCardType.SelectedItem.Equals(""))
                {
                    MessageBox.Show("読込むカードの種類を選択してください");
                    return;
                }
                //Felica読込
                WindowManager.Instance.ReadFelcia(this.ComboICCardType.SelectedItem.ToString());

                LabelResult.Content = "Great";
                LabelResult.Background = Brushes.Blue;
                //読込んだ履歴をDataGridに表示
                this.DataGridCardResult.ItemsSource = WindowManager.Instance.ReadHistoryList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LabelResult.Content = "Nooon";
                LabelResult.Background = Brushes.Red;
            }
        }

        /// <summary>
        /// 月集計の結果をDataGridItemsourceにセット
        /// プロセスを選択していない、又は空欄の場合はProcessは全件検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDBViewMonth_Click(object sender, RoutedEventArgs e)
        {
            if (ComboSelectProcess.SelectedItem == null)
            {
                ComboSelectProcess.SelectedIndex = 0;
            }
            WindowManager.Instance.AggregateResults(ResultPeriod ,CalendarDaysSelect.DisplayDate,ComboSelectProcess.SelectedItem.ToString());
            this.DataGridDBResult.ItemsSource = WindowManager.Instance.ResultList;
        }

        //集計で使用するProcessの読み込み
        private void TabDataView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //データ項目Processの種類ごとの集計を行う
            //コンボボックスに種類項目を挿入
            if (File.Exists(Properties.Settings.Default.UserDBFilePath))
            {
                var processList = WindowManager.Instance.SearchProcesses();
                this.ComboSelectProcess.ItemsSource = processList;
            }
            else
            {
                //this.ComboSelectProcess.SelectedIndex = 0;
                ComboSelectProcess.SelectedIndex = 0;
            }
        }

        private void ButtonResultPeriod_Click(object sender, RoutedEventArgs e)
        {
            if (ResultPeriod == false)
            {
                CalendarDaysSelect.DisplayMode = CalendarMode.Year;
                ResultPeriod = true;
                this.ButtonResultPeriod.Content = "週集計";
            }
            else if(ResultPeriod == true)
            {
                CalendarDaysSelect.DisplayMode = CalendarMode.Decade;
                ResultPeriod = false;
                this.ButtonResultPeriod.Content = "月集計";
            }
        }


    }
}
