using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace andbeans.Models
{
    public class DataCache
    {
        static MemoryCache _cache = MemoryCache.Default;
        // Now uses ConcurrentDictionary, which is thread-safe and should avoid CPU locking issues
        static ConcurrentDictionary<string, CacheItemInfo> _cacheInfo = new ConcurrentDictionary<string, CacheItemInfo>();

        public List<CacheItemInfo> BaseCache
        {
            get
            {
                return _cache.Select(item => {
                    if (_cacheInfo.ContainsKey(item.Key))
                    {
                        return _cacheInfo[item.Key];
                    }
                    else
                    {
                        return new CacheItemInfo() { Key = "Not Found: " + item.Key, Hits = -1, Misses = 0, Type = "" };
                    }
                }).ToList();
            }
        }

        public object this[string key]
        {
            get { return _cache[key]; }
        }

        public void Add(string key, object value, int expirationSeconds)
        {
            _cache.Add(key,
                       value,
                       new CacheItemPolicy
                       {
                           AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationSeconds)
                       });

            CacheItemInfo cacheItemInfo = null;

            if (_cacheInfo.TryGetValue(key, out cacheItemInfo))
            {
                cacheItemInfo.Misses++;
            }
            else
            {
                _cacheInfo.TryAdd(key, new CacheItemInfo
                {
                    Key = key,
                    Type = value.GetType().ToString(),
                    Hits = 0,
                    Misses = 1
                });
            }
        }

        public T Get<T>(string key)
        {
            CacheItemInfo cacheItemInfo = null;

            if (_cacheInfo.TryGetValue(key, out cacheItemInfo))
            {
                cacheItemInfo.Hits++;
                return (T)_cache[key];
            }

            return default(T);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);

            // ConcurrentDictionary has no Remove method, and for some reason the TryRemove method 
            // needs to populate an output parameter, hence this slightly odd code
            CacheItemInfo unused;
            _cacheInfo.TryRemove(key, out unused);
        }

        public void Clear()
        {
            List<KeyValuePair<String, Object>> cacheItems = (from n in _cache.AsParallel() select n).ToList();

            foreach (KeyValuePair<String, Object> a in cacheItems)
                _cache.Remove(a.Key);

            _cacheInfo.Clear();
        }
    }
}