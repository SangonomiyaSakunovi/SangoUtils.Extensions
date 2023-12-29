using System.Diagnostics;
using System.Text;

namespace SangoScripts_Server.Logger
{
    public class LoggerUtils_Sango
    {
        private static LoggerConfig_Sango? _config;
        private static ILogger_Sango? _logger;
        private static StreamWriter? _logFileWriter = null;

        public static void InitSettings(LoggerConfig_Sango? cfg = null)
        {
            cfg ??= new LoggerConfig_Sango();
            _config = cfg;
            switch (_config.loggerType)
            {
                case LoggerType.OnConsole:
                    _logger = new ConsoleLogger();
                    break;
            }
            if (_config.enableSaveLog == false) { return; }
            if (_config.enableCoverLog)
            {
                string path = _config.saveLogPath + _config.saveLogName;
                try
                {
                    if (Directory.Exists(_config.saveLogPath))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(_config.saveLogPath);
                    }
                    _logFileWriter = File.AppendText(path);
                    _logFileWriter.AutoFlush = true;
                }
                catch
                {
                    _logFileWriter = null;
                }
            }
            else
            {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-ss");
                string path = _config.saveLogPath + prefix + _config.saveLogName;
                try
                {
                    if (Directory.Exists(_config.saveLogPath) == false)
                    {
                        Directory.CreateDirectory(_config.saveLogPath);
                    }
                    _logFileWriter = File.AppendText(path);
                    _logFileWriter.AutoFlush = true;
                }
                catch
                {

                }
            }
        }

        #region Log
        public static void LogInfo(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                log = DecorateLog(string.Format(log, args));
                _logger?.Log(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
        }

        public static void LogInfo(object logObj)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger?.Log(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
        }

        public static void ColorLog(LoggerColor color, string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                log = DecorateLog(string.Format(log, args));
                _logger?.Log(log, color);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
        }

        public static void ColorLog(LoggerColor color, object logObj)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger?.Log(log, color);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
        }

        public static void LogTraceInfo(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                log = DecorateLog(string.Format(log, args), true);
                _logger?.Log(log, LoggerColor.Magenta);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogTraceInfo]{0}", log));
                }
            }
        }

        public static void LogTraceInfo(object logObj)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString(), true);
                _logger?.Log(log, LoggerColor.Magenta);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogTraceInfo]{0}", log));
                }
            }
        }

        public static void LogWarn(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                log = DecorateLog(string.Format(log, args));
                _logger?.Warn(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogWarn]{0}", log));
                }
            }
        }

        public static void LogWarn(object logObj)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger?.Warn(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogWarn]{0}", log));
                }
            }
        }

        public static void LogError(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                log = DecorateLog(string.Format(log, args));
                _logger?.Error(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogError]{0}", log));
                }
            }
        }

        public static void LogError(object logObj)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger?.Error(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogError]{0}", log));
                }
            }
        }

        public static void LogProcessing(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                log = DecorateLog(string.Format(log, args));
                _logger?.Processing(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogProcessing]{0}", log));
                }
            }
        }

        public static void LogProcessing(object logObj)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger?.Processing(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogProcessing]{0}", log));
                }
            }
        }

        public static void LogDone(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                log = DecorateLog(string.Format(log, args));
                _logger?.Done(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogDone]{0}", log));
                }
            }
        }

        public static void LogDone(object logObj)
        {
            if (_config != null)
            {
                if (_config.enableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger?.Done(log);
                if (_config.enableSaveLog)
                {
                    WriteToFile(string.Format("[LogDone]{0}", log));
                }
            }
        }
        #endregion

        #region Decorate
        private static string DecorateLog(string? log, bool isTraceInfo = false)
        {
            if (_config != null && !string.IsNullOrEmpty(log))
            {
                StringBuilder sb = new StringBuilder(_config.logPrefix, 100);
                if (_config.enableTimestamp)
                {
                    sb.AppendFormat(" {0}", DateTime.Now.ToString("hh:mm:ss--fff"));
                }
                if (_config.enableThreadID)
                {
                    sb.AppendFormat(" {0}", GetThreadID());
                }
                sb.AppendFormat(" {0} {1}", _config.logSeparate, log);
                if (isTraceInfo)
                {
                    sb.AppendFormat(" \nStackTrace: {0}", GetTraceInfo());
                }
                return sb.ToString();
            }
            return "";
        }

        private static string GetThreadID()
        {
            return string.Format("ThreadID:{0}", Environment.CurrentManagedThreadId);
        }

        private static string GetTraceInfo()
        {
            StackTrace st = new(3, true);    //The method called DecorateLog has 3 calls should be ignore
            string traceInfo = "";
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame? sf = st.GetFrame(i);
                if (sf != null)
                {
                    traceInfo += string.Format("\n    {0}::{1}  line:{2}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber());
                }
            }
            return traceInfo;
        }
        #endregion

        private static void WriteToFile(string log)
        {
            if (_logFileWriter != null)
            {
                try
                {
                    _logFileWriter.WriteLine(log);
                }
                catch
                {
                    _logFileWriter = null;
                }
            }
        }

        private class ConsoleLogger : ILogger_Sango
        {
            public void Log(string log, LoggerColor color = LoggerColor.None)
            {
                ConsoleLog(log, color);
            }

            public void Processing(string log)
            {
                ConsoleLog(log, LoggerColor.Cyan);
            }

            public void Done(string log)
            {
                ConsoleLog(log, LoggerColor.Green);
            }

            public void Warn(string log)
            {
                ConsoleLog(log, LoggerColor.Yellow);
            }

            public void Error(string log)
            {
                ConsoleLog(log, LoggerColor.Red);
            }

            private void ConsoleLog(string log, LoggerColor color)
            {
                switch (color)
                {
                    case LoggerColor.None:
                        Console.WriteLine(log);
                        break;
                    case LoggerColor.Yellow:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LoggerColor.Red:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LoggerColor.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LoggerColor.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LoggerColor.Magenta:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LoggerColor.Cyan:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    default:
                        Console.WriteLine(log);
                        break;
                }
            }
        }
    }
}
