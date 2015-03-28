using System;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace YooPoon.Core.Cache
{
    public class MemoryCacheManager:ICacheManager
    {
        protected ObjectCache Cache
        {
            get
            {
                //使用System.Runtime.Caching下的MemoryCache
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// 依据键获取缓存的内容值
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>与键相关的值</returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 将对象按指定的键保存到缓存中
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">对象</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// 检查缓存中时候已存在拥有该键的缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>结果</returns>
        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 从缓存中删除指定缓存项
        /// </summary>
        /// <param name="key">key</param>
        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 依据正则表达式从缓存中删除符合规则的缓存项
        /// </summary>
        /// <param name="pattern">pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = (from item in Cache where regex.IsMatch(item.Key) select item.Key).ToList();

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}