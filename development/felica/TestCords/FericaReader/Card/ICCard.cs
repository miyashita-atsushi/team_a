using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FericaReader
{
    [Table(Name = "UserHistory")]
    class ICCard
    {
        //マッピング用
        [Column(Name = "date")]
        public string Date              { get; set; }//日付
        [Column(Name="terminal")]
        public string   Terminal        { get; set; }//端末
        [Column(Name="process")]
        public string   Process         { get; set; }//処理
        [Column(Name="payment")]
        public int      Payment         { get; set; }//出金額
        [Column(Name="deposit")]
        public int      Deposit         { get; set; }//入金額
        [Column(Name ="balance")]
        public int      Balance         { get; set }//残高
        [Column(Name="inStationName")]
        public string   InStationName   { get; set; }//入駅名
        [Column(Name="outStationName")]
        public string   OutStationName  { get; set; }//出駅名
        [Column(Name="idm")]
        public string   IDm             { get; set; }//IDm
        [Column(Name="transactionID")]
        public string   TransactionID   { get; set; }//取引ID

        //フィールド
        protected StationCode stCode;

        
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



    }
}
