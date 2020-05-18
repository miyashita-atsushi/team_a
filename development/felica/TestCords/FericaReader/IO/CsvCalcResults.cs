using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FericaReader.IO
{
    /// <summary>
    /// 集計結果のCSVクラス
    /// </summary>
    class CsvCalcResults
    {
        public string   IDm         { get; set; }//IDm
        public DateTime FromDate    { get; set; }//集計開始日時
        public DateTime ToDate      { get; set; }//集計終了日時
        public int      Value       { get; set; }//残高?

    }
}
