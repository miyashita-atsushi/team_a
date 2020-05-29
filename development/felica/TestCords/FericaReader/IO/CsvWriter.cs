using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace FericaReader
{
    class CsvWriter
    {
        //もしCSVHelperのマッピングを使うのであれば以下をどうぞ
        private class SuicaMapper : CsvHelper.Configuration.ClassMap<ICCard>
        {

        }

        public static void WriteResultCsv(List<ICCard> cardList)
        {

        }

        public static void WriteCalucResultCsv(List<CsvCalcResults> cardList)
        {

        }
    }
}
