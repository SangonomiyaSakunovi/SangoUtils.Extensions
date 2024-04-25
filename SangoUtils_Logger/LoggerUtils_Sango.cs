using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SangoUtils.Loggers
{
    public class LoggerUtils_Sango
    {
        private static LoggerConfig_Sango? _config;
        private static ILogger_Sango _logger = new ConsoleLogger();
        private static StreamWriter? _logFileWriter;

        public static void InitSettings(LoggerConfig_Sango? cfg = null)
        {
            cfg ??= new LoggerConfig_Sango();
            _config = cfg;
            switch (_config.LoggerType)
            {
                case LoggerType.OnWindowConsole:
                    _logger = new ConsoleLogger();
                    break;
#if UNITY_ENV
                case LoggerType.OnUnityConsole:
                    _logger = new UnityLogger();
                    break;
#endif
            }
            if (_config.EnableSaveLog == false) { return; }
            if (_config.EnableCoverLog)
            {
                string path = _config.SaveLogPath + _config.SaveLogName;
                try
                {
                    if (Directory.Exists(_config.SaveLogPath))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(_config.SaveLogPath);
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
                string path = _config.SaveLogPath + prefix + _config.SaveLogName;
                try
                {
                    if (Directory.Exists(_config.SaveLogPath) == false)
                    {
                        Directory.CreateDirectory(_config.SaveLogPath);
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
                if (_config.EnableSangoLog == false) { return; }
                if (args != null && args.Length > 0)
                {
                    log = DecorateLog(string.Format(log, args));
                }
                _logger.Log(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogInfo(object logObj)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger.Log(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void ColorLog(LoggerColor color, string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                if (args != null && args.Length > 0)
                {
                    log = DecorateLog(string.Format(log, args));
                }
                _logger.Log(log, color);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void ColorLog(LoggerColor color, object logObj)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger.Log(log, color);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogInfo]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogTraceInfo(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                if (args != null && args.Length > 0)
                {
                    log = DecorateLog(string.Format(log, args));
                }
                _logger.Log(log, LoggerColor.Magenta);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogTraceInfo]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogTraceInfo(object logObj)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString(), true);
                _logger.Log(log, LoggerColor.Magenta);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogTraceInfo]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogWarn(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                if (args != null && args.Length > 0)
                {
                    log = DecorateLog(string.Format(log, args));
                }
                _logger.Warn(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogWarn]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogWarn(object logObj)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger.Warn(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogWarn]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogError(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                if (args != null && args.Length > 0)
                {
                    log = DecorateLog(string.Format(log, args));
                }
                _logger.Error(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogError]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogError(object logObj)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger.Error(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogError]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogProcessing(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                if (args != null && args.Length > 0)
                {
                    log = DecorateLog(string.Format(log, args));
                }
                _logger.Processing(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogProcessing]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogProcessing(object logObj)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger.Processing(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogProcessing]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogDone(string log, params object[] args)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                if (args != null && args.Length > 0)
                {
                    log = DecorateLog(string.Format(log, args));
                }
                _logger.Done(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogDone]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        public static void LogDone(object logObj)
        {
            if (_config != null)
            {
                if (_config.EnableSangoLog == false) { return; }
                string log = DecorateLog(logObj.ToString());
                _logger.Done(log);
                if (_config.EnableSaveLog)
                {
                    WriteToFile(string.Format("[LogDone]{0}", log));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }
        #endregion

        #region Decorate
        private static string DecorateLog(string log, bool isTraceInfo = false)
        {
            if (_config != null)
            {
                StringBuilder sb = new StringBuilder(_config.LogPrefix, 100);
                if (_config.EnableTimestamp)
                {
                    sb.AppendFormat(" {0}", DateTime.Now.ToString("hh:mm:ss--fff"));
                }
                if (_config.EnableThreadID)
                {
                    sb.AppendFormat(" {0}", GetThreadID());
                }
                sb.AppendFormat(" {0} {1}", _config.LogSeparate, log);
                if (isTraceInfo)
                {
                    sb.AppendFormat(" \nStackTrace: {0}", GetTraceInfo());
                }
                return sb.ToString();
            }
            else
            {
                throw new ArgumentNullException(nameof(_config));
            }
        }

        private static string GetThreadID()
        {
            return string.Format("ThreadID:{0}", Environment.CurrentManagedThreadId);
        }

        private static string GetTraceInfo()
        {
            StackTrace st = new StackTrace(3, true);    //The method called DecorateLog has 3 calls should be ignore
            string traceInfo = "";
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                traceInfo += string.Format("\n    {0}::{1}  line:{2}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber());
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

#if UNITY_ENV
        private class UnityLogger : ILogger_Sango
        {
            public void Log(string log, LoggerColor color = LoggerColor.None)
            {
                log = GetUnityLogColorString(log, color);
                UnityEngine.Debug.Log(log);
            }

            public void Processing(string log)
            {
                log = GetUnityLogColorString(log, LoggerColor.Cyan);
                UnityEngine.Debug.Log(log);
            }

            public void Done(string log)
            {
                log = GetUnityLogColorString(log, LoggerColor.Green);
                UnityEngine.Debug.Log(log);
            }

            public void Warn(string log)
            {
                log = GetUnityLogColorString(log, LoggerColor.Yellow);
                UnityEngine.Debug.Log(log);
            }

            public void Error(string log)
            {
                log = GetUnityLogColorString(log, LoggerColor.Red);
                UnityEngine.Debug.Log(log);
            }

            private string GetUnityLogColorString(string log, LoggerColor color)
            {
                switch (color)
                {
                    case LoggerColor.None:
                        break;
                    case LoggerColor.Yellow:
                        log = string.Format("<color=#FFFF00>{0}</color>", log);
                        break;
                    case LoggerColor.Red:
                        log = string.Format("<color=#FF0000>{0}</color>", log);
                        break;
                    case LoggerColor.Green:
                        log = string.Format("<color=#00FF00>{0}</color>", log);
                        break;
                    case LoggerColor.Blue:
                        log = string.Format("<color=#0000FF>{0}</color>", log);
                        break;
                    case LoggerColor.Magenta:
                        log = string.Format("<color=#FF00FF>{0}</color>", log);
                        break;
                    case LoggerColor.Cyan:
                        log = string.Format("<color=#00FFFF>{0}</color>", log);
                        break;
                }
                return log;
            }
        }
#endif
    }
}