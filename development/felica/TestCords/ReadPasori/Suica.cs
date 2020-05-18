using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadPasori
{
    /// <summary>
    /// Suica以外を読み込むため、本来は基底クラスを作成し継承する
    /// </summary>
    class Suica
    {
        //ここでは駅改札の場合のみを考えています
        //物販などは処理ごとで日付、残高などのバイト列が異なる

        public int TerminalCode         { get; set; }//byte[0]　端末種
        public int ProcessCode          { get; set; }//byte[1]　処理
        public int GetStation       { get; set; }//byte[3]　入出場種別
        //public DateTime Date        { get; set; }//byte[4,5]　日付　bitで(7/4/5)(年/月/日)
        public int InStationLineCode    { get; set; }//byte[6]　入線区
        public int InStationCode        { get; set; }//byte[7]　入駅
        public int OutStationLineCode   { get; set; }//byte[8]　出線区
        public int OutStationCode       { get; set; }//byte[9]  出駅
        public int Balance          { get; set; }//byte[10,11]　残高(little endian)
        public int Seq              { get; set; }//byte[12,13,15]　連番
        public int Region           { get; set; }//byte[15]　地域コード
        //ここから下の変数をDB、CSVに保存
        //public DateTime Date            { get; set; }//byte[4,5]　日付　bitで(7/4/5)(年/月/日)
        public string Date            { get; set; }//byte[4,5]　日付　bitで(7/4/5)(年/月/日)
        public string   ByteData        { get; set; } 
        public byte[]   Binary          { get; set; }
        public string   Terminal        { get; set; }//端末
        public string   Process         { get; set; }//処理
        public int      Payment         { get; set; }//入金額
        public int      Deposit         { get; set; }//出金額
        public string   InStationName   { get; set; }//入駅名
        public string   OutStationName  { get; set; }//出駅名

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
                    this.Deposit = Math.Abs(value);
                }
                else
                {
                    this.Payment = value;
                }
            }
        }

        private StationCode stCode;
        /// <summary>
        /// 処理
        /// </summary>
        public enum PROCESS_NUMBERS : byte
        {
            運賃              = 1,
            チャージ          = 2,
            券購1             = 3,
            清算              = 5,
            窓出              = 6,
            新規              = 7,
            控除              = 8,
            バス_Pitapa       = 13,
            バス_Iruca        = 15,
            再発行            = 17,
            支払              = 19,
            オートチャージ    = 21,
            バスチャージ      = 31,
            券購2             = 35,
            物販              = 70,
            特典              = 72,
            入金              = 73,
            物販取消          = 74,
            入場物販          = 75,
            精算_他社         = 132,
            精算_他社入場     = 133,
            物販_現金併用     = 198,
            物販_入場現金併用 = 203
        }
        public enum TERMINAL_NUMBERS : byte
        {
            精算機           =3 ,
            携帯型端末       =4 ,
            車載端末         =5 ,
            券売機1            =7 ,
            券売機2           =8 ,
            入金機           =9 ,
            券売機          =18 ,
            券売機等1        =20 ,
            券売機等2        =21 ,
            改札機          =22 ,
            簡易改札機      =23 ,
            窓口端末1        =24,
            窓口端末2        =25 ,
            改札端末        =26 ,
            携帯電話        =27 ,
            乗継精算機      =28 ,
            連絡改札機      =29 ,
            簡易入金機      =31 ,
            VIEWALTTE1      =70 ,
            VIEWALTTE2      =72 ,
            物販端末       =199 ,
            自販機         =200 

        }

         private const int CT_SHOP = 199;  // 物販端末
        private const int CT_VEND = 200;  // 自販機
        private const int CT_CAR = 5;   // 車載端末(バス)
        /// <summary>
        ///重複する値があるのでこちらを使用
        ///とりあえずMainWindowで使ってるのでPublic
        /// </summary>
        /// <param name="ctype"></param>
        public string consoleType(int ctype)
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
        public string procType(int proc)
        {
            switch (proc) {
            case 1: return "運賃";
            case 2: return "チャージ";
            case 3: return "券購";
            case 4: 
            case 5: return "清算";
            case 6: return "窓出";
            case 7: return "新規";
            case 8: return "控除";
            case 13: return "バス"; //PiTaPa系
            case 15: return "バス"; //IruCa系
            case 17: return "再発行";
            case 19: return "支払";
            case 20:
            case 21: return "オートチャージ";
            case 31: return "バスチャージ";
            case 35: return "券購";
            case 70: return "物販";
            case 72: return "特典";
            case 73: return "入金";
            case 74: return "物販取消";
            case 75: return "入場物販";
            case 132: return "精算(他社)";
            case 133: return "精算(他社入場)";
            case 198: return "物販(現金併用)";
            case 203: return "物販(入場現金併用)";
            }
            return "不明";
        }
        /// <summary>
        /// 
        /// </summary>
        public Suica()
        {

        }

        public void analyzeTransaction(byte[] data)
        {
            stCode = new StationCode();

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

            this.Binary = data;

            this.Date = AnaryzeDateTime(read2b(data,4));          
            this.ByteData =  BitConverter.ToString(data, 0, 16);
            this.Terminal = consoleType(TerminalCode);
            this.Process = procType(ProcessCode);
            int inArea = getAreaCode(InStationLineCode,Region);
            int outArea = getAreaCode(OutStationLineCode,Region);
            this.InStationName = stCode.GetStationName(inArea,InStationLineCode,InStationCode);
            this.OutStationName = stCode.GetStationName(outArea,OutStationLineCode,OutStationCode);

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
        public string AnaryzeDateTime(int date)
        {
            //年は2000年から数えて
            int yy = (date >> 9) + 2000;
            int mm = (date >> 5) & 0xf;
            int dd = date & 0x1f;
            try
            {
               return new DateTime(yy, mm, dd).ToString("yyyy/MM/dd");
            }
            catch
            {
                //nullの場合
                return new DateTime(0,0,0).ToString("yyyy/mm/dd");
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
