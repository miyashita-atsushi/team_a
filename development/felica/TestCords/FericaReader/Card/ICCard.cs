using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FericaReader
{
    [Table(Name = "UserHistory")]
    abstract class ICCard
    {
        //マッピング用
        [Column(Name = "date")]
        public string   Date            { get; set; }//日付
        [Column(Name="terminal")]
        public string   Terminal        { get; set; }//端末
        [Column(Name="process")]
        public string   Process         { get; set; }//処理
        [Column(Name="payment")]
        public int      Payment         { get; set; }//出金額
        [Column(Name="deposit")]
        public int      Deposit         { get; set; }//入金額
        [Column(Name ="balance")]
        public int      Balance         { get; set; }//残高
        [Column(Name="inStationName")]
        public string   InStationName   { get; set; }//入駅名
        [Column(Name="outStationName")]
        public string   OutStationName  { get; set; }//出駅名
        [Column(Name="idm")]
        public string   IDm             { get; set; }//IDm
        [Column(Name="transactionID")]
        public string   TransactionID   { get; set; }//取引ID

        private const int CT_SHOP = 199;  // 物販端末
        private const int CT_VEND = 200;  // 自販機
        private const int CT_CAR = 5;   // 車載端末(バス)
        /// <summary>
        ///重複する値があるのでこちらを使用
        /// </summary>
        /// <param name="ctype"></param>
        protected string consoleType(int ctype)
        {
            switch (ctype) {
            //case CT_SHOP: return "物販端末";
            //case CT_VEND: return "自販機";
            //case CT_CAR: return "車載端末";
            case 199: return "物販端末";
            case 200: return "自販機";
            case 5: return "車載端末";
            case 3: return "清算機";
            case 4: return "携帯型端末";
            case 8: return "券売機";
            case 9: return "入金機";
            case 18: return "券売機";
            case 20:
            case 21: return "券売機等";
            case 22: return "改札機";
            case 23: return "簡易改札機";
            case 24:
            case 25: return "窓口端末";
            case 26: return "改札端末";
            case 27: return "携帯電話";
            case 28: return "乗継精算機";
            case 29: return "連絡改札機";
            case 31: return "簡易入金機";
            case 70:
            case 72: return "VIEW ALTTE";
            }
            return "不明";
        }

        // 処理
        protected string procType(int proc)
        {
            switch (proc) {
            case 1  : return "運賃";
            case 2  : return "チャージ";
            case 3  : return "券購";
            case 4  : 
            case 5  : return "清算";
            case 6  : return "窓出";
            case 7  : return "新規";
            case 8  : return "控除";
            case 13 : return "バス"; //PiTaPa系
            case 15 : return "バス"; //IruCa系
            case 17 : return "再発行";
            case 19 : return "支払";
            case 20 :
            case 21 : return "オートチャージ";
            case 31 : return "バスチャージ";
            case 35 : return "券購";
            case 70 : return "物販";
            case 72 : return "特典";
            case 73 : return "入金";
            case 74 : return "物販取消";
            case 75 : return "入場物販";
            case 132: return "精算(他社)";
            case 133: return "精算(他社入場)";
            case 198: return "物販(現金併用)";
            case 203: return "物販(入場現金併用)";
            }
            return "不明";
        }
        
        //出入金判断
        public int Value {
            set
            {
                if(value == 0)
                {
                    this.Payment = 0;
                    this.Deposit = 0;
                }
                else if(value < 0)
                {
                    this.Payment = Math.Abs(value);
                }
                else
                {
                    this.Deposit = value;
                }
            }
        }
        /// <summary>
        /// 履歴の重複をIDを作成
        /// IDm + 読込んだデータ列
        /// </summary>
        /// <param name="data"></param>
        protected void CreateTransactionID(byte[] data)
        {
            this.TransactionID = this.IDm ;
            foreach(var d in data)
            {
                this.TransactionID += d.ToString("X2");
            }
        }

        public virtual void AnalyzeTransaction(byte[] data)
        {
            //派生クラスで実装

        }

        public virtual string AnaryzeDateTime(int date)
        {
            //派生クラスで実装
            return "0";
        }

    }
}
