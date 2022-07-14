using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Core.Cache
{
    public interface ICache
    {
        /// <summary>
        /// 判定指定键是否存在
        /// </summary>
        /// <param name="key">指定的键</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 判定指定键是否存在
        /// </summary>
        /// <param name="key">指定的键</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 移除指定键
        /// </summary>
        /// <param name="key">指定的键</param>
        /// <returns></returns>
        bool Remove(string key);

        /// <summary>
        /// 移除指定键
        /// </summary>
        /// <param name="key">指定的键</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// 批量移除指定键
        /// </summary>
        /// <param name="keys">指定的键集合</param>
        /// <returns>成功移除的key的个数</returns>
        long RemoveAll(IEnumerable<string> keys);

        /// <summary>
        /// 批量移除指定键
        /// </summary>
        /// <param name="keys">指定的键集合</param>
        /// <returns>成功移除的key的个数</returns>
        Task<long> RemoveAllAsync(IEnumerable<string> keys);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TValue">缓存的值类型</typeparam>
        /// <param name="key">指定的键</param>
        /// <param name="value">要缓存的值</param>
        /// <returns></returns>
        bool Set<TValue>(string key, TValue value, TimeSpan? expiry = null);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TValue">缓存的值类型</typeparam>
        /// <param name="key">指定的键</param>
        /// <param name="value">要缓存的值</param>
        /// <returns></returns>
        Task<bool> SetAsync<TValue>(string key, TValue value, TimeSpan? expiry = null);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TValue">返回值类型</typeparam>
        /// <param name="key">指定的键</param>
        /// <returns></returns>
        TValue Get<TValue>(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TValue">返回值类型</typeparam>
        /// <param name="key">指定的键</param>
        /// <returns></returns>
        Task<TValue> GetAsync<TValue>(string key);
    }
}
