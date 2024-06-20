using System;

namespace SangoUtils.Bases
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
    }
}