using System;

namespace SangoUtils_Bases_Universal
{
    public abstract class Singleton<T> where T : class
    {
        private static T? _instance;
        private static readonly object _lock = new object();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                }
                return _instance;
            }
        }
        public static void Dispose()
        {
            lock (_lock)
            {
                if (_instance != null)
                {
                    _instance = null;
                }
            }
        }
    }

    public abstract class Singleton<T, K> where T : class where K : class
    {
        private static T? _instance;
        private static readonly object _lock = new object();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                }
                return _instance;
            }
        }
        public static void Dispose()
        {
            lock (_lock)
            {
                if (_instance != null)
                {
                    _instance = null;
                }
            }
        }
    }

    public abstract class Singleton<T, K, L> where T : class where K : class where L : class
    {
        private static T? _instance;
        private static readonly object _lock = new object();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                }
                return _instance;
            }
        }
        public static void Dispose()
        {
            lock (_lock)
            {
                if (_instance != null)
                {
                    _instance = null;
                }
            }
        }
    }

    public abstract class Singleton<T, K, L, M> where T : class where K : class where L : class where M : class
    {
        private static T? _instance;
        private static readonly object _lock = new object();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                }
                return _instance;
            }
        }
        public static void Dispose()
        {
            lock (_lock)
            {
                if (_instance != null)
                {
                }
            }
        }
    }
}