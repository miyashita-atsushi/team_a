using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FericaReader
{
    class WindowManager
    {
        private static WindowManager Instance = new WindowManager();

        //本来はICカード基底クラスのリストにする
        private List<ICCard> HistoryList = new List<ICCard>();

        private WindowManager()
        {

        }
        public static WindowManager GetInstance()
        {
            return Instance;
        }
        public List<ICCard> GetHistoryList()
        {
            return HistoryList;
        }

        public void WriteUserHistoryDB()
        {
            //リスト反転
            //this.HistoryList.Reverse();
            //CalcuValue();
            IO.GetInstance().WhiteUserHistorySql(this.HistoryList);
            //IO.GetInstance().WriteCsv(this.HistoryList);
            //書き込んだら初期化
            this.HistoryList = new List<ICCard>();
        }

        /// <summary>
        /// リスト追加時にIDmセット
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idm"></param>
        public void AddHistryList(byte[] data ,string idm)
        {
            var suica = new Suica();
            suica.IDm = idm;
            suica.analyzeTransaction(data);
            HistoryList.Add(suica);
        }

        /// <summary>
        /// Suicaは出入金自体の値を保存していないので最初に読み込んだ残額から
        /// 現在の残額を計算しなければならない
        /// そのため最古の履歴は計算できないので削除し、残高だけを利用する
        /// </summary>
        /// <returns></returns>
        public void CalcuValue()
        {
            int prevBalance = 0;
            this.HistoryList.Reverse();

            foreach (Suica t in this.HistoryList)
            {
                t.Value = t.Balance - prevBalance;
                prevBalance = t.Balance;
            }
            HistoryList.RemoveAt(0);   // 最古の履歴は捨てる
        }
    }
}
