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
    public class Log4netManager
    {
        public static Log4netManager Instance = new Log4netManager();
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

        public Log4netManager()
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            rootLogger = ((Hierarchy)logger.Logger.Repository).Root;

            LogLevelDic.TryGetValue(Properties.Settings.Default.LogLevel, out Level level);
            rootLogger.Level = level;

            //appender = rootLogger.GetAppender("RollingLogFileAppender") as FileAppender;
            //appender.Layout = new PatternLayout("%date [%thread] %-5level %logger [%property{NDC}] - %message%newline");
            //appender.File = @Properties.Settings.Default.LogFilePath;
            //appender.ActivateOptions();
        }

    }
}
