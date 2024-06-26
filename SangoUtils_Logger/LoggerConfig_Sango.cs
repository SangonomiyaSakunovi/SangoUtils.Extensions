using System;

namespace SangoUtils.Loggers
{
    public class LoggerConfig_Sango
    {
        public bool EnableSangoLog { get; set; } = true;
        public string LogPrefix { get; set; } = "#";
        public bool EnableTimestamp { get; set; } = true;
        public string LogSeparate { get; set; } = ">>";
        public bool EnableThreadID { get; set; } = true;
        public bool EnableTraceInfo { get; set; } = true;
        public bool EnableSaveLog { get; set; } = true;
        public bool EnableCoverLog { get; set; } = true;
        public string SaveLogPath { get; set; } = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory);
        public string SaveLogName { get; set; } = "SangoLog.txt";
        public LoggerType LoggerType { get; set; } = LoggerType.OnWindowConsole;
    }

    public enum LoggerType
    {
        OnWindowConsole,
        OnUnityConsole
    }

    public enum LoggerColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

    internal interface ILogger_Sango
    {
        void Log(string message, LoggerColor color = LoggerColor.None);
        void Processing(string message);
        void Done(string message);
        void Warn(string message);
        void Error(string message);
    }
}