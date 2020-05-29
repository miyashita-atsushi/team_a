using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Linq.Mapping;

namespace FericaReader
{
    /// <summary>
    /// Suica以外を読み込むため、実装時は基底クラスを作成し継承する
    /// 
    /// ここで実装しているクラスは取得バイト列のフォーマットが駅改札の場合のみを考えています
    /// バス利用・物販などは処理ごとで日付、残高などのフォーマットが異なる為
    /// 実装時は履歴内のProcess・Terminalで分岐を判断する
    /// </summary>
    [Table(Name = "UserHistory")]
     class Suica : ICCard
    {
        public const int SERVICE_SUICA_INOUT = 0x108f;
        //履歴読込のサービスコード
        public const int SERVICE_SUICA_HISTORY = 0x090f;

        public int TerminalCode      ;
        public int ProcessCode       ;
        public int GetStation        ;
        public int InStationLineCode ;
        public int InStationCode     ;
        public int OutStationLineCode;
        public int OutStationCode    ;
        public int Seq               ;
        public int Region            ;
        //public int Balance           ;
        //public int TerminalCode         { get; set; }//byte[0]　端末種
        //public int ProcessCode          { get; set; }//byte[1]　処理
        //public int GetStation           { get; set; }//byte[3]　入出場種別
        //public DateTime Date            { get; set; }//byte[4,5]　日付　bitで(7/4/5)(年/月/日)
        //public int InStationLineCode    { get; set; }//byte[6]　入線区
        //public int InStationCode        { get; set; }//byte[7]　入駅
        //public int OutStationLineCode   { get; set; }//byte[8]　出線区
        //public int OutStationCode       { get; set; }//byte[9]  出駅
        //public int Balance              { get; set; }//byte[10,11]　残高(little endian)
        //public int Seq                  { get; set; }//byte[12,13,15]　連番
        //public int Region               { get; set; }//byte[15]　地域コード

         /// <summary>
        /// 
        /// </summary>
        public Suica()
        {

        }

        public override void AnalyzeTransaction(byte[] data)
        {
            this.TerminalCode = data[0];
            this.ProcessCode = data[1];       
            this.GetStation = data[3];    
            this.InStationLineCode = data[6]; 
            this.InStationCode = data[7]; 
            this.OutStationLineCode = data[8];
            this.OutStationCode = data[9];
            this.Region = data[15];

            this.Balance = read2l(data,10);

            //this.Seq = data[];            ToDO:3byte読込メソッド作成

            this.Date = AnaryzeDateTime(read2b(data,4));          
            //this.ByteData =  BitConverter.ToString(data, 0, 16);
            this.Terminal = consoleType(TerminalCode);
            this.Process = procType(ProcessCode);
            if (ProcessCode == 1)
            {
                int inArea = getAreaCode(InStationLineCode,Region);
                int outArea = getAreaCode(OutStationLineCode,Region);
                this.InStationName = DataBaseAccess.GetStationName(inArea,InStationLineCode,InStationCode);
                this.OutStationName = DataBaseAccess.GetStationName(outArea,OutStationLineCode,OutStationCode);
            }

            CreateTransactionID(data);
        }

        //2バイト読込(BigEndian)
        protected int read2b(byte[] b, int pos)
        {
            int ret = b[pos] << 8 | b[pos + 1];
            return ret;
        }
        //2バイト読込(littleEndian)
         protected int read2l(byte[] b, int pos)
        {
            int ret = b[pos + 1] << 8 | b[pos];
            return ret;
        }
        //public DateTime AnaryzeDateTime(int date)
        //public string AnaryzeDateTime(int date)
        public override string AnaryzeDateTime(int date)
        {
            //年は2000年から数えて
            int yy = (date >> 9) + 2000;
            int mm = (date >> 5) & 0xf;
            int dd = date & 0x1f;
            try
            {
                
                return new DateTime(yy, mm, dd).ToString("yyyy-MM-dd");
                //return new DateTime(yy, mm, dd);
            }
            catch
            {
                //nullの場合
                return new DateTime(0, 0, 0).ToString("yyyy-MM-dd");
                //return new DateTime(0,0,0);
            }
        }
        /// <summary>
        /// 駅コードの為、エリアコード取得
        /// </summary>
        /// <param name="line"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private int getAreaCode(int line, int region)
        {
            var areacodeSoruce = (byte)(0x3 & region >> 4);
            if ((line & 0xFF) < 128)
                return 0;
            if (region >= 0 && region <= 3)
                return (byte)(region + 1);
            return 0;
        }

    }
}
