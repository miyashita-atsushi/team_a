using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using log4net.Core;
using System.Reflection;
using log4net.Layout;

namespace FericaReader
{
    class Log4netManager
    {
        private static Log4netManager Instance;
        public ILog logger;
        private Logger rootLogger;
        private FileAppender appender;
        private Dictionary<string, Level> LogLevelDic = new Dictionary<string, Level>()
        {
            {"TRACE",Level.Trace },//詳細出力
            {"DEBUG",Level.Debug },//開発用メッセージ
            {"INFO",Level.Info },//操作ログ
            {"WARN",Level.Warn },//警告
            {"ERROR",Level.Error },//システム停止ではない問題となる障害
            {"FATAL",Level.Fatal }//システム停止するような致命的な障害
        };

        private Log4netManager()
        {
            logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().FullName);
            
            rootLogger = ((Hierarchy)logger.Logger.Repository).Root;
            
            LevelSet();
            appender = rootLogger.GetAppender("RollingLogFileAppender") as FileAppender;
            //appender.Layout = new PatternLayout("%date [%thread] %-5level %logger [%property{NDC}] - %message%newline");
            //appender.File = @"\samplelogger";
            //appender.ActivateOptions();
            //rootLogger.Level = Level.
        }

        public static Log4netManager GetInstance()
        {
            if (null == Instance)
            {
                Instance = new Log4netManager();
            }
            return Instance;
        }

        public void LevelSet()
        {
            if (LogLevelDic.TryGetValue(Properties.Settings.Default.LogLevel, out Level level))
            {
                rootLogger.Level = level;
            }
            else
            {
                Console.WriteLine(string.Format("設定したログレベル{0}は設定出来ません", Properties.Settings.Default.LogLevel));
                rootLogger.Level = Level.All;
            }
        }

        public void ReadConfig()
        {

        }

    }
}
