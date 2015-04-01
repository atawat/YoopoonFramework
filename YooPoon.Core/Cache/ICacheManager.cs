using YooPoon.Core.Autofac;

namespace YooPoon.Core.Cache
{
    public interface ICacheManager : IDependency
    {
        /// <summary>
        /// 依据键获取缓存的内容值
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>与键相关的值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 将对象按指定的键保存到缓存中
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">对象</param>
        /// <param name="cacheTime">缓存时间</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// 检查缓存中时候已存在拥有该键的缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>结果</returns>
        bool IsSet(string key);

        /// <summary>
        /// 从缓存中删除指定缓存项
        /// </summary>
        /// <param name="key">key</param>
        void Remove(string key);

        /// <summary>
        /// 依据正则表达式从缓存中删除符合规则的缓存项
        /// </summary>
        /// <param name="pattern">pattern</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();
    }
}