using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FericaReader
{
    /// <summary>
    /// LinqでProcessデータベースから取得するためのクラス
    /// Linqを使わない場合はいらない
    /// 絞り込み検索を行うコンボボックスに値を入れるため
    /// </summary>
    [Table(Name = "ProcessTable")]
    class ProcessDBTable
    {
        [Column(Name = "processNumber")]
        public int ProcessNumber { get; set;}
        [Column(Name = "processName")]
        public string ProcessName { get; set;}

    }
}
