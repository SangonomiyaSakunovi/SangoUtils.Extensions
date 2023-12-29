using SangoScripts_Server.Logger;

namespace SangoUtils_Server.Config
{
    public static class SangoSystemConfig
    {
        public static readonly LoggerConfig_Sango LoggerConfig_Sango = new LoggerConfig_Sango
        {
            enableSangoLog = true,
            logPrefix = "#",
            enableTimestamp = true,
            logSeparate = ">>",
            enableThreadID = true,
            enableTraceInfo = true,
            enableSaveLog = true,
            enableCoverLog = true,
            saveLogPath = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory),
            saveLogName = "SangoLog.txt",
            loggerType = LoggerType.OnConsole
        };
    }
}
