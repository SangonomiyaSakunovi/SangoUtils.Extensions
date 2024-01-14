using System;
using System.Threading;

#if UNITY_ENV
using UnityEngine;
#endif

namespace SangoUtils_IOCP
{
    public class IOCPLogger
    {
        private static BaseIOCPLogger _logger;

        public static Action<string> LogInfoCallBack { get; set; }
        public static Action<string> LogErrorCallBack { get; set; }
        public static Action<string> LogWarningCallBack { get; set; }

        public static void SetLogger(IOCPRunnerType runnerType)
        {
            switch (runnerType)
            {
                case IOCPRunnerType.ConsoleProject:
                    _logger = new IOCPConsoleLogger();
                    break;
                case IOCPRunnerType.UnityProject:
                    _logger = new IOCPUnityLogger();
                    break;
            }
        }

        public static void Info(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                _logger.Log(message, IOCPLogColor.None);
            }
        }

        public static void Start(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                _logger.Log(message, IOCPLogColor.Blue);
            }
        }

        public static void Special(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                _logger.Log(message, IOCPLogColor.Magenta);
            }
        }

        public static void Done(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                _logger.Log(message, IOCPLogColor.Green);
            }
        }

        public static void Processing(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                _logger.Log(message, IOCPLogColor.Cyan);
            }
        }

        public static void Error(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogErrorCallBack != null)
            {
                LogErrorCallBack(message);
            }
            else
            {
                _logger.Log(message, IOCPLogColor.Red);
            }
        }

        public static void Warning(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogWarningCallBack != null)
            {
                LogWarningCallBack(message);
            }
            else
            {
                _logger.Log(message, IOCPLogColor.Yellow);
            }
        }

        private abstract class BaseIOCPLogger 
        {
            public abstract void Log(string message, IOCPLogColor color);
        }

        private class IOCPConsoleLogger : BaseIOCPLogger
        {
            public override void Log(string message, IOCPLogColor color)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                message = string.Format("Thread:{0} {1}", threadId, message);
                switch (color)
                {
                    case IOCPLogColor.None:
                        Console.WriteLine(message);
                        break;
                    case IOCPLogColor.Yellow:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case IOCPLogColor.Red:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case IOCPLogColor.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case IOCPLogColor.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case IOCPLogColor.Magenta:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case IOCPLogColor.Cyan:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    default:
                        Console.WriteLine(message);
                        break;
                }
            }
        }

#if UNITY_ENV
        private class IOCPUnityLogger : BaseIOCPLogger
        {
            public override void Log(string message, IOCPLogColor color)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                message = string.Format("Thread:{0} {1}", threadId, message);
                switch (color)
                {
                    case IOCPLogColor.None:
                        UnityEngine.Debug.Log(message);
                        break;
                    case IOCPLogColor.Yellow:
                        message = string.Format("<color=#FFFF00>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case IOCPLogColor.Red:
                        message = string.Format("<color=#FF0000>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case IOCPLogColor.Green:
                        message = string.Format("<color=#00FF00>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case IOCPLogColor.Blue:
                        message = string.Format("<color=#0000FF>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case IOCPLogColor.Magenta:
                        message = string.Format("<color=#FF00FF>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case IOCPLogColor.Cyan:
                        message = string.Format("<color=#00FFFF>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    default:
                        UnityEngine.Debug.Log(message);
                        break;
                }
            }
        }
#endif
    }

    public enum IOCPLogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

    public enum IOCPRunnerType
    {
        ConsoleProject,
        UnityProject
    }
}
