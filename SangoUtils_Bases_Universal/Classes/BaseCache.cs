using System;
using System.Collections.Generic;

namespace SangoUtils.Bases
{
    public abstract class BaseCache
    {
        public string CacheId { get; protected set; } = "";

        public string Id { get; protected set; } = "";

        public CacheLevelCode CacheLevelCode { get; protected set; }

        private static readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        //TO Check
        public static void Add<T>(T obj)
        {
            var type = typeof(T);
            if (_cache.ContainsKey(type))
            {
                _cache[type] = obj;
            }
            else
            {
                _cache.Add(type, obj);
            }
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (_cache.ContainsKey(type))
            {
                return (T)_cache[type];
            }
            return default;
        }

        public static void Remove<T>()
        {
            var type = typeof(T);
            if (_cache.ContainsKey(type))
            {
                _cache.Remove(type);
            }
        }

        public static void Clear()
        {
            _cache.Clear();
        }
    }

    public enum CacheLevelCode
    {
        Root,
    }
}
