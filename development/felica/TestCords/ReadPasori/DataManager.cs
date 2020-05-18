using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadPasori
{
    class DataManager
    {
        private static DataManager Instance = new DataManager();

        //本来はICカード基底クラスのリストを所持する
        private List<Suica> HistoryList = new List<Suica>();

        private DataManager()
        {

        }
        public static DataManager GetInstance()
        {
            return Instance;
        }
        public List<Suica> GetHistoryList()
        {
            return HistoryList;
        }

        public void WriteUserHistoryDB()
        {
            //リスト反転
            this.HistoryList.Reverse();
            CalcuValue();
            IO.GetInstance().WhiteUserHistorySql(this.HistoryList);
            IO.GetInstance().WriteCsv(this.HistoryList);
        }

        public void AddHistryList(byte[] data)
        {
            var suica = new Suica();
            suica.analyzeTransaction(data);
            HistoryList.Add(suica);
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

            foreach (Suica t in this.HistoryList)
            {
                t.Value = t.Balance - prevBalance;
                prevBalance = t.Balance;
            }
            HistoryList.RemoveAt(0);   // 最古の履歴は捨てる
        }
    }
}
