using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FericaReader
{
    /// <summary>
    /// 集計結果のCSVクラス
    /// </summary>
    public class CsvCalcResults
    {
        public string   IDm         { get; set; }//IDm
        public DateTime FromDate    { get; set; }//集計開始日時
        public DateTime ToDate      { get; set; }//集計終了日時
        public int      Deposit       { get; set; }//入金
        public int      Payment       { get; set; }//出金
        public string   FromToDate    { get;set;}
    }
}

//    /// <summary>
//    /// 集計結果のCSVクラス
//    /// </summary>
//    public class CsvCalcResults  : INotifyPropertyChanged
//    {

//        private string   _IDm        ;//IDm
//        private DateTime _FromDate   ;//集計開始日時
//        private DateTime _ToDate     ;//集計終了日時
//        private int      _Deposit    ;//入金
//        private int      _Payment    ;//出金
//        private string   _FromToDate ;

//        public string   IDm             
//        {
//            get{ return _IDm;} 
//            set
//            { 
//                _IDm = value;
//                OnPropertyChanged("IDm");
//            } 
//        }

//        public DateTime FromDate          {
//            get{ return _FromDate;} 
//            set
//            { 
//                _FromDate = value;
//                OnPropertyChanged("FromDate");
//            } 
//        }
//        public DateTime ToDate            {
//            get{ return _ToDate;} 
//            set
//            { 
//                _ToDate = value;
//                OnPropertyChanged("ToDate");
//            } 
//        }
//        public int      Deposit           {
//            get{ return _Deposit;} 
//            set
//            { 
//                _Deposit = value;
//                OnPropertyChanged("Deposit");
//            } 
//        }
//        public int      Payment           {
//            get{ return _Payment;} 
//            set
//            { 
//                _Payment = value;
//                OnPropertyChanged("Payment");
//            } 
//        }
//        public string   FromToDate        {
//            get{ return _FromToDate;} 
//            set
//            { 
//                _FromToDate = value;
//                OnPropertyChanged("FromToDate");
//            } 
//        }


//        public event PropertyChangedEventHandler PropertyChanged;
//        private void OnPropertyChanged(string propertyName)
//        {
//            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
