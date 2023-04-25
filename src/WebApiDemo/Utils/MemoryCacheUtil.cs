using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Utils
{
    /// <summary>
    /// 缓存
    /// 缓存数据存储在内存中
    /// </summary>
    public static class MemoryCacheUtil
    {
        #region 变量
        /// <summary>
        /// 内存缓存
        /// </summary>
        private static ConcurrentDictionary<string, CacheData> _cacheDict = new ConcurrentDictionary<string, CacheData>();

        /// <summary>
        /// 对不同的键提供不同的锁，用于读缓存
        /// </summary>
        private static ConcurrentDictionary<string, string> _dictLocksForReadCache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 过期缓存检测Timer
        /// </summary>
        private static System.Timers.Timer _timerCheckCache;
        #endregion

        #region 静态构造函数
        static MemoryCacheUtil()
        {
            _timerCheckCache = new System.Timers.Timer();
            _timerCheckCache.Interval = 10 * 60 * 1000;
            _timerCheckCache.Elapsed += _timerCheckCache_Elapsed;
            _timerCheckCache.Start();
        }
        #endregion

        #region 获取并缓存数据
        /// <summary>
        /// 获取并缓存数据
        /// 高并发的情况建议使用此重载函数，防止重复写入内存缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="func">在此方法中初始化数据</param>
        /// <param name="expirationSeconds">缓存过期时间(秒)，0表示永不过期</param>
        /// <param name="refreshCache">立即刷新缓存</param>
        public static T TryGetValue<T>(string cacheKey, Func<T> func, int expirationSeconds = 0, bool refreshCache = false)
        {
            lock (_dictLocksForReadCache.GetOrAdd(cacheKey, cacheKey))
            {
                object cacheValue = MemoryCacheUtil.GetValue(cacheKey);
                if (cacheValue != null && !refreshCache)
                {
                    return (T)cacheValue;
                }
                else
                {
                    T value = func();
                    MemoryCacheUtil.SetValue(cacheKey, value, expirationSeconds);
                    return value;
                }
            }
        }
        #endregion

        #region 获取并缓存数据(异步)
        /// <summary>
        /// 获取并缓存数据
        /// 高并发的情况建议使用此重载函数，防止重复写入内存缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="func">在此方法中初始化数据</param>
        /// <param name="expirationSeconds">缓存过期时间(秒)，0表示永不过期</param>
        /// <param name="refreshCache">立即刷新缓存</param>
        public static async Task<T> TryGetValueAsync<T>(string cacheKey, Func<ValueTask<T>> func, int expirationSeconds = 0, bool refreshCache = false)
        {
            object cacheValue = MemoryCacheUtil.GetValue(cacheKey);
            if (cacheValue != null && !refreshCache)
            {
                return (T)cacheValue;
            }
            else
            {
                T value = await func(); //因为异步不方便加锁,此处可能会被多次执行
                MemoryCacheUtil.SetValue(cacheKey, value, expirationSeconds);
                return value;
            }
        }
        #endregion

        #region SetValue 保存键值对
        /// <summary>
        /// 保存键值对
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="expirationSeconds">过期时间(秒)，0表示永不过期</param>
        public static void SetValue(string key, object value, int expirationSeconds = 0)
        {
            try
            {
                CacheData data = new CacheData(key, value);
                data.updateTime = DateTime.Now;
                data.expirationSeconds = expirationSeconds;

                CacheData temp;
                _cacheDict.TryRemove(key, out temp);
                _cacheDict.TryAdd(key, data);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex, "MemoryCacheUtil写缓存错误");
            }
        }
        #endregion

        #region GetValue 获取键值对
        /// <summary>
        /// 获取键值对
        /// </summary>
        public static object GetValue(string key)
        {
            try
            {
                CacheData data;
                if (_cacheDict.TryGetValue(key, out data))
                {
                    if (data.expirationSeconds > 0 && DateTime.Now.Subtract(data.updateTime).TotalSeconds > data.expirationSeconds)
                    {
                        CacheData temp;
                        _cacheDict.TryRemove(key, out temp);
                        return null;
                    }
                    return data.value;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex, "MemoryCacheUtil读缓存错误");
                return null;
            }
        }
        #endregion

        #region Delete 删除缓存
        /// <summary>
        /// 删除缓存
        /// </summary>
        internal static void Delete(string key)
        {
            CacheData temp;
            _cacheDict.TryRemove(key, out temp);
        }
        #endregion

        #region DeleteAll 删除全部缓存
        /// <summary>
        /// 删除全部缓存
        /// </summary>
        internal static void DeleteAll()
        {
            _cacheDict.Clear();
        }
        #endregion

        #region 过期缓存检测
        /// <summary>
        /// 过期缓存检测
        /// </summary>
        private static void _timerCheckCache_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    foreach (string cacheKey in _cacheDict.Keys.ToList())
                    {
                        CacheData data;
                        if (_cacheDict.TryGetValue(cacheKey, out data))
                        {
                            if (data.expirationSeconds > 0 && DateTime.Now.Subtract(data.updateTime).TotalSeconds > data.expirationSeconds + 3600 * 24)
                            {
                                CacheData temp;
                                string strTemp;
                                _cacheDict.TryRemove(cacheKey, out temp);
                                _dictLocksForReadCache.TryRemove(cacheKey, out strTemp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex, "过期缓存检测出错");
                }
            });
        }
        #endregion

    }

    #region CacheData 缓存数据
    /// <summary>
    /// 缓存数据
    /// </summary>
    [Serializable]
    public class CacheData
    {
        /// <summary>
        /// 键
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object value { get; set; }
        /// <summary>
        /// 缓存更新时间
        /// </summary>
        public DateTime updateTime { get; set; }
        /// <summary>
        /// 过期时间(秒)，0表示永不过期
        /// </summary>
        public int expirationSeconds { get; set; }

        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        public CacheData(string key, object value)
        {
            this.key = key;
            this.value = value;
        }
    }
    #endregion

}
