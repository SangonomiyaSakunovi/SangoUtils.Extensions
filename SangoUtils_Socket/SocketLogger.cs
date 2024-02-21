using System;
using System.Threading;

#if UNITY_ENV
using UnityEngine;
#endif

namespace SangoUtils_Socket
{
    public class SocketLogger
    {
        private static BaseSocketLogger _logger;

        public static Action<string> LogInfoCallBack { get; set; }
        public static Action<string> LogErrorCallBack { get; set; }
        public static Action<string> LogWarningCallBack { get; set; }

        public static void SetLogger(SocketRunnerType runnerType)
        {
            switch (runnerType)
            {
                case SocketRunnerType.ConsoleProject:
                    _logger = new SocketConsoleLogger();
                    break;
                case SocketRunnerType.UnityProject:
                    _logger = new SocketUnityLogger();
                    break;
            }
        }

        public static void Info(string message, params object[] arguments)
        {
            if (_logger != null)
            {
                message = string.Format(message, arguments);
                if (LogInfoCallBack != null)
                {
                    LogInfoCallBack(message);
                }
                else
                {
                    _logger.Log(message, SocketLogColor.None);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_logger));
            }
        }

        public static void Start(string message, params object[] arguments)
        {
            if (_logger != null)
            {
                message = string.Format(message, arguments);
                if (LogInfoCallBack != null)
                {
                    LogInfoCallBack(message);
                }
                else
                {
                    _logger.Log(message, SocketLogColor.Blue);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_logger));
            }
        }

        public static void Special(string message, params object[] arguments)
        {
            if (_logger != null)
            {
                message = string.Format(message, arguments);
                if (LogInfoCallBack != null)
                {
                    LogInfoCallBack(message);
                }
                else
                {
                    _logger.Log(message, SocketLogColor.Magenta);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_logger));
            }
        }

        public static void Done(string message, params object[] arguments)
        {
            if (_logger != null)
            {
                message = string.Format(message, arguments);
                if (LogInfoCallBack != null)
                {
                    LogInfoCallBack(message);
                }
                else
                {
                    _logger.Log(message, SocketLogColor.Green);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_logger));
            }
        }

        public static void Processing(string message, params object[] arguments)
        {
            if (_logger != null)
            {
                message = string.Format(message, arguments);
                if (LogInfoCallBack != null)
                {
                    LogInfoCallBack(message);
                }
                else
                {
                    _logger.Log(message, SocketLogColor.Cyan);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_logger));
            }
        }

        public static void Error(string message, params object[] arguments)
        {
            if (_logger != null)
            {
                message = string.Format(message, arguments);
                if (LogErrorCallBack != null)
                {
                    LogErrorCallBack(message);
                }
                else
                {
                    _logger.Log(message, SocketLogColor.Red);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_logger));
            }
        }

        public static void Warning(string message, params object[] arguments)
        {
            if (_logger != null)
            {
                message = string.Format(message, arguments);
                if (LogWarningCallBack != null)
                {
                    LogWarningCallBack(message);
                }
                else
                {
                    _logger.Log(message, SocketLogColor.Yellow);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(_logger));
            }
        }

        private abstract class BaseSocketLogger
        {
            public abstract void Log(string message, SocketLogColor color);
        }

        private class SocketConsoleLogger : BaseSocketLogger
        {
            public override void Log(string message, SocketLogColor color)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                message = string.Format("Thread:{0} {1}", threadId, message);
                switch (color)
                {
                    case SocketLogColor.None:
                        Console.WriteLine(message);
                        break;
                    case SocketLogColor.Yellow:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case SocketLogColor.Red:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case SocketLogColor.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case SocketLogColor.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case SocketLogColor.Magenta:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case SocketLogColor.Cyan:
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
        private class SocketUnityLogger : BaseSocketLogger
        {
            public override void Log(string message, SocketLogColor color)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                message = string.Format("Thread:{0} {1}", threadId, message);
                switch (color)
                {
                    case SocketLogColor.None:
                        UnityEngine.Debug.Log(message);
                        break;
                    case SocketLogColor.Yellow:
                        message = string.Format("<color=#FFFF00>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case SocketLogColor.Red:
                        message = string.Format("<color=#FF0000>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case SocketLogColor.Green:
                        message = string.Format("<color=#00FF00>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case SocketLogColor.Blue:
                        message = string.Format("<color=#0000FF>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case SocketLogColor.Magenta:
                        message = string.Format("<color=#FF00FF>{0}</color>", message);
                        UnityEngine.Debug.Log(message);
                        break;
                    case SocketLogColor.Cyan:
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

    public enum SocketLogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

    public enum SocketRunnerType
    {
        ConsoleProject,
        UnityProject
    }
}
