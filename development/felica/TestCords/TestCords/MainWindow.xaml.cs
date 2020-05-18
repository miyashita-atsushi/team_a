using System;
using System.Collections.Generic;
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
using System.Data.SQLite;
using System.Data.SQLite.Linq;

namespace TestCords
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string DBFileName = "testDB.sqlite"; 
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            using(var conn = new SQLiteConnection ("Data Source=" + DBFileName))
            {
                conn.Open();
                using(SQLiteCommand command = conn.CreateCommand())
                {
                    var sb = new StringBuilder();
                    sb.Append("CREATE TABLE IF NOT EXISTS test(");
                    sb.Append("date TEXT NOT NULL ,");
                    sb.Append("type TEXT NOT NULL ,");
                    sb.Append("money INTEGER NOT NULL ,");
                    sb.Append("getonStation TEXT ,");
                    sb.Append("getoffStation TEXT");
                    sb.Append(")");

                    command.CommandText = sb.ToString();
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        //データベースからテーブル取得
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new SQLiteConnection("Data Source =" + DBFileName))
            {
                conn.Open();
                using(var dataset = new DataSet())
                {
                    string Sql = "SELECT date ,type ,money ,getonStation ,getoffStation From test ORDER BY date DESC LIMIT 1000";

                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(Sql,conn);
                    dataAdapter.Fill(dataset);
                    //データグリッドに表示
                    this.DataGridTest.AutoGenerateColumns = false;
                    this.DataGridTest.DataContext = dataset.Tables[0].DefaultView;
                }
                conn.Close();
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            AddDataToDB();
        }

        private void AddDataToDB()
        {
            using(var conn = new SQLiteConnection("Data Source =" + DBFileName))
            {
                conn.Open();
                using(var dataset = new DataSet())
                {
                    String sql = string.Format("INSERT INTO test(date,type,money,getonStation,getoffStation) VALUES('{0}','{1}','{2}','{3}','{4}')",
                                                DateTime.Now,this.TextBoxType.Text,int.Parse(this.TextBoxMoney.Text),this.TextBoxGetonStation.Text,this.TextBoxGetoffStation.Text);
                    var dataAdapter = new SQLiteDataAdapter(sql,conn);
                    dataAdapter.Fill(dataset);
                }
                conn.Close();
            }
        }
    }
    public class ResultData
    {
        public int PrimaryNum { get;set;}
       
    }
}
