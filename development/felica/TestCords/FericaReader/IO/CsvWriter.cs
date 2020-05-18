using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace FericaReader.IO
{
    class CsvWriter
    {
        
        public CsvWriter GetInstance()
        {

        }
        //もしCSVHelperのマッピングを使うのであれば以下をどうぞ
        private class SuicaMapper : CsvHelper.Configuration.ClassMap<ICCard>
        {

        }

        private void WriteResultCsv(List<ICCard> cardList)
        {

        }

        private void WriteCalucResultCsv(List<CsvCalcResults> cardList)
        {

        }
    }
}
