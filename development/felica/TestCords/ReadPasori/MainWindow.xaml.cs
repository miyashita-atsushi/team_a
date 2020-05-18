using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.Services3.Messaging;
using PCSC;
using PCSC.Iso7816;

namespace ReadPasori
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _selectedIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCheckReader_Click(object sender, RoutedEventArgs e)
        {
            textBox_Log.Clear(); 
            comboBox_CardReader.Items.Clear();
            //接続確立
            using(var ctx = ContextFactory.Instance.Establish(SCardScope.User))
            {
                var firstReader = ctx.GetReaders().FirstOrDefault();
                if (firstReader == null)
                {
                    textBox_Log.Text += "No reader connected."; //③
                    return;
                }
                using (var reader = ctx.ConnectReader(firstReader, SCardShareMode.Direct, SCardProtocol.Unset))
                {
                    var status = reader.GetStatus();
 
                    textBox_Log.Text += @"Reader names: {" + string.Join(", ", status.GetReaderNames()) + "}\r\n"; //④以下4行
                    textBox_Log.Text += @"Protocol: {" + status.Protocol + "}\r\n";
                    textBox_Log.Text += @"State: {" + status.State + "}\r\n";
                    textBox_Log.Text += @"ATR: { " + BitConverter.ToString(status.GetAtr() ?? new byte[0]) + "}\r\n";
                    textBox_Log.Text += @"ATR: { " + NoBarATR(status.GetAtr()) + "}\r\n";
 
                    //comboBox_CardReader.Items.Add(status.GetReaderNames());//⑤
                    status.GetReaderNames().ToList().ForEach(item => comboBox_CardReader.Items.Add(item));
                }
            }
            textBox_Log.Text += "-----------------------------------------------\r\n"; //⑥
        }

        private void ComboBox_CardReader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedIndex = comboBox_CardReader.SelectedIndex;
        }

        private void Button_ReadID_Click(object sender, RoutedEventArgs e)
        {
            var contextFactory = ContextFactory.Instance;

            using(var context = contextFactory.Establish(SCardScope.System))
            {
                var readerNames = context.GetReaders();
                if (NoReaderFound(readerNames))
                {
                    textBox_Log.Text += "You need at least one reader in order to run this example." + "\r\n";
                    return;
                }

                var  readerName = ChooseRfidReader(readerNames);

                if(readerName == null) return;
                using(var rfidReader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
                {
                    var apdu = new CommandApdu(IsoCase.Case2Short, rfidReader.Protocol)
                    {
                        CLA = 0xFF,
                        Instruction = InstructionCode.GetData,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0
                    };
                    using (rfidReader.Transaction(SCardReaderDisposition.Leave))
                    {
                        textBox_Log.Text += "Retrieving the UID .... " + "\r\n";

                        var sendPci         = SCardPCI.GetPci(rfidReader.Protocol);
                        var receivePci      = new SCardPCI();
                        var receiveBuffer   = new byte[256];
                        var command         = apdu.ToArray();

                        var bytesReceived   = rfidReader.Transmit(
                            sendPci,
                            command,
                            command.Length,
                            receivePci,
                            receiveBuffer,
                            receiveBuffer.Length);
                        var responseApdu = new ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, rfidReader.Protocol);

                        textBox_Log.Text += "SW1: " + responseApdu.SW1.ToString() + " ,SW2: " + responseApdu.SW2.ToString() +"\r\n";

                        if (responseApdu.HasData)
                        {
                            textBox_Log.Text += "Uid: " + BitConverter.ToString(responseApdu.GetData()) + "\r\n";
                        }
                        else
                        {
                            textBox_Log.Text += "Uid: No uid received" + "\r\n";
                        }
                    }
                }
            }
            textBox_Log.Text += "-----------------------------------------------\r\n";
        }

        private void Button_ReadCardType_Click(object sender, RoutedEventArgs e)
        {
            var contextFactory = ContextFactory.Instance;
 
            using (var context = contextFactory.Establish(SCardScope.System))
            {
                var readerNames = context.GetReaders();
                if (NoReaderFound(readerNames))
                {
                    textBox_Log.Text += "You need at least one reader in order to run this example." + "\r\n";
                    return;
                }
 
                var readerName = ChooseRfidReader(readerNames);
                if (readerName == null)
                {
                    return;
                }
 
                // 'using' statement to make sure the reader will be disposed (disconnected) on exit
                using (var rfidReader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
                {
                    var apdu = new CommandApdu(IsoCase.Case2Short, rfidReader.Protocol)
                    {
                        CLA = 0xFF,
                        Instruction = InstructionCode.GetData,
                        P1 = 0xF3,
                        P2 = 0x00,
                        Le = 0 // We don't know the ID tag size
                    };
 
                    using (rfidReader.Transaction(SCardReaderDisposition.Leave))
                    {
                        textBox_Log.Text += "Retrieving the UID .... " + "\r\n";
 
                        var sendPci = SCardPCI.GetPci(rfidReader.Protocol);
                        var receivePci = new SCardPCI(); // IO returned protocol control information.
 
                        var receiveBuffer = new byte[256];
                        var command = apdu.ToArray();
 
                        var bytesReceived = rfidReader.Transmit(
                            sendPci, // Protocol Control Information (T0, T1 or Raw)
                            command, // command APDU
                            command.Length,
                            receivePci, // returning Protocol Control Information
                            receiveBuffer,
                            receiveBuffer.Length); // data buffer
 
                        var responseApdu =
                            new ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, rfidReader.Protocol);
                        textBox_Log.Text += "SW1: " + responseApdu.SW1.ToString()
                                                + ", SW2: " + responseApdu.SW2.ToString()
                                                + "\r\n";
                        if (responseApdu.HasData)
                        {
                            textBox_Log.Text += "CardType: " + BitConverter.ToString(responseApdu.GetData()) + "\r\n";
                        }
                        else
                        {
                            textBox_Log.Text += "Uid: No uid received" + "\r\n";
                        }
 
                    }
                }
            }
            textBox_Log.Text += "-----------------------------------------------\r\n";
        }

        /// <summary>
        /// 不要なものも含まれているので実装時は選別
        /// 現在は履歴を一件ずつ解析、表示しているが処理金額比較などを行う為
        /// データクラスを作成して、一度すべて読み込んでから解析を行う事
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SuicaRead_Click(object sender, RoutedEventArgs e)
        {
            var contextFactory = ContextFactory.Instance;
            using(var context = contextFactory.Establish(SCardScope.System))
            {
                var readerNames = context.GetReaders();
                if (NoReaderFound(readerNames))
                {
                    textBox_Log.Text += "You need at least one reader in order to run this example.\r\n";
                    return;
                }
                var readerName = ChooseRfidReader(readerNames);
                if(readerName == null)return;

                using(var rfidReader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
                {
                    byte[] dataIn = { 0x0f,0x09 };
                    var apduSelectFile = new CommandApdu(IsoCase.Case4Short, rfidReader.Protocol)
                    {
                        CLA = 0xFF,
                        Instruction = InstructionCode.SelectFile,
                        P1 = 0x00,
                        P2 = 0x01,
                        Data = dataIn,
                        Le = 0
                    };
                    using (rfidReader.Transaction(SCardReaderDisposition.Leave))
                    {
                        textBox_Log.Text += "SelectFile .... \r\n";

                        var sendPci = SCardPCI.GetPci(rfidReader.Protocol);
                        var receivePci = new SCardPCI();
                        var receiveBuffer = new byte[256];
                        var command = apduSelectFile.ToArray();

                        var bytesReceivedSelectedFile = rfidReader.Transmit(sendPci,command,command.Length,receivePci,receiveBuffer,receiveBuffer.Length);
                        var responseApdu = new ResponseApdu(receiveBuffer,bytesReceivedSelectedFile,IsoCase.Case2Short,rfidReader.Protocol);

                        textBox_Log.Text += "SW1: " + responseApdu.SW1.ToString()
                                         + ", SW2: " + responseApdu.SW2.ToString()+ "\r\n"
　　　　　　　　　　　　　　　　　　　　 + "Length: " + responseApdu.Length.ToString() + "\r\n";

                        for (int i = 0; i < 20; ++i)
                        {
                            var apduReadBinary = new CommandApdu(IsoCase.Case2Short, rfidReader.Protocol)
                            {
                                CLA = 0xFF,
                                Instruction = InstructionCode.ReadBinary,
                                P1 = 0x00,
                                P2 = (byte)i,
                                Le = 0
                            };
                            textBox_Log.Text += "Read Binary .... \r\n";

                            var commandReadBinary = apduReadBinary.ToArray();

                            var bytesReceivedReadBinary2 =
                                rfidReader.Transmit(sendPci, commandReadBinary, commandReadBinary.Length, receivePci, receiveBuffer, receiveBuffer.Length);
                            var responseApdu2 = new ResponseApdu(receiveBuffer, bytesReceivedReadBinary2, IsoCase.Case2Extended, rfidReader.Protocol);

                            textBox_Log.Text += "SW1: " + responseApdu2.SW1.ToString()
                                        + ", SW2: " + responseApdu2.SW2.ToString()
                                        + "\r\n"
                                        + "Length: " + responseApdu2.Length.ToString() + "\r\n";

                            //parse_tag(receiveBuffer);
                            DataManager.GetInstance().AddHistryList(receiveBuffer);

                            textBox_Log.Text += "\r\n";
                        }
                        //履歴DB、CSVに書き込み
                        DataManager.GetInstance().WriteUserHistoryDB();
                        foreach (var s in DataManager.GetInstance().GetHistoryList())
                        {
                            ShowResultData(s);
                        }
                    }
                }
            }
            textBox_Log.Text += "-----------------------------------------------\r\n";
        }

        //ATR(Answer To Reset)ICカードの仕様定義
        private string NoBarATR(byte[] arr)
        {
            if(arr == null)return string.Empty;
            string result = string.Empty;

            foreach (var oneByte in arr)
            {
                result += String.Format("{0:X2}",oneByte) + " ";
            }
            return result;
        }

        private bool NoReaderFound(ICollection<string> readerNames)
        {
            if(readerNames == null || readerNames.Count == 0)
            {
                return true;
            }
            return false;
        }
        //コンボボックスで選択したカードリーダー名を取得、チェック
        private string ChooseRfidReader(IList<string> readerNames)
        {
            textBox_Log.Text += "Available readers: ";
            for(var i = 0 ;i < readerNames.Count ; i++)
            {
                textBox_Log.Text += "ReaderID: " + i.ToString() + " ,Reader Name:" + readerNames[i] + "\r\n";
            }
            textBox_Log.Text += "Which reader is an RFID reader? \r\n";

            int choice  = _selectedIndex;
            if(choice >= 0 && (choice <= readerNames.Count))
            {
                return readerNames[choice];
            }
            textBox_Log.Text += "An invalid number has been entered.\r\n";
            textBox_Log.Text += "-----------------------------------------------\r\n";
            return null;
        }

        private void parse_tag(byte[] data)
        {
            //var suica = new Suica();
            //suica.analyzeTransaction(data);
            //ShowResultData(suica);
            DataManager.GetInstance().AddHistryList(data);
            //textBox_Log.Text += "Suica履歴データ：" + BitConverter.ToString(data, 0, 16) + "\r\n";
            //ShowResultData(suica);
        }
        private void setTextLog(string msg)
        {
            if (Dispatcher.CheckAccess()) {
                textBox_Log.Text += msg +"\r\n";
            }
            else {
                Dispatcher.Invoke((Action)(() =>
                {
                    textBox_Log.Text += msg + "\r\n";
                }));
            }
        }
        private void ShowResultData(Suica suica)
        {
            setTextLog("時間: " + suica.Date);
            setTextLog("処理: " + suica.Process);
            setTextLog("入駅: " + suica.InStationName);
            setTextLog("出駅: " + suica.OutStationName);
            setTextLog("InLineCode: " + suica.InStationLineCode);
            setTextLog("InStationCode: " + suica.InStationCode);
            setTextLog("OutLineCode: "  + suica.OutStationLineCode);
            setTextLog("OutStationCode:" + suica.OutStationCode);
        }

        private void Button_ServiceTest_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
