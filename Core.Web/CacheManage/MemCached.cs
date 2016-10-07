using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Core.Web.CacheManage.Memcached;

namespace Core.Web.CacheManage
{
    /// <summary>
    /// 基于MemCache的缓存
    /// </summary>
    public class MemCache : ICache
    {
        private static MemCache defaultCache = null;
        private static object cacheLock = new object();

        #region ICache 成员

        /// <summary>
        /// 获取默认的缓存
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public static MemCache GetCacheService(bool enable)
        {
            if (defaultCache == null)
            {
                lock (cacheLock)
                {
                    if (defaultCache == null)
                    {
                        defaultCache = new MemCache("Cache", new string[] { "192.168.1.146:11211" }, enable);
                    }
                }
            }
            return defaultCache;
        }
        private MemcachedClient cache;

        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="servers">服务器</param>
        /// <param name="enable">是否可用</param>
        public MemCache(string name, string[] servers, bool enable)
        {
            if (!MemcachedClient.Exists(name))
            {
                MemcachedClient.Setup(name, servers);
            }
            cache = MemcachedClient.GetInstance(name);
            this.enable = enable;
        }

        private bool enable = false;
        /// <summary>
        /// 获取或设置一个值,指示当前的缓存是否可用
        /// </summary>
        public bool EnableCache
        {
            get
            {
                return this.enable;
            }
            set
            {
                this.enable = value;
            }
        }

        /// <summary>
        /// 获取缓存的类型
        /// </summary>
        public CacheType Type
        {
            get
            {
                return CacheType.MemCached;
            }
        }

        /// <summary>
        /// 检查缓存中是否存在指定的键
        /// </summary>
        /// <param name="key">要检查的键</param>
        /// <returns>返回一个值,指示检查的键是否存在</returns>
        public bool Contains(string key)
        {
            if (this.enable)
            {
                return cache.Get(key) != null;
            }
            return false;
        }

        public bool Contains<T>(string key)
        {
            object value = cache.Get(key);
            if (value is T)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 从缓存中获取指定键的值
        /// </summary>
        /// <param name="key">要获取的键</param>
        /// <returns>返回指定键的值</returns>
        public T Get<T>(string key)
        {
            if (this.enable)
            {
                return (T)cache.Get(key);
            }
            return default(T);
        }

        /// <summary>
        /// 尝试返回指定的缓存
        /// </summary>
        /// <typeparam name="T">缓存内容的类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <param name="value">缓存的内容</param>
        /// <returns>是否存在这个缓存</returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            object temp = cache.Get(key);
            if (temp != null && temp is T)
            {
                value = (T)temp;
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// 获取缓存中键值的数量
        /// </summary>
        public int Count
        {
            get
            {
                if (this.enable)
                {
                    throw new NotImplementedException();
                }
                return 0;
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        public void Add<T>(string key, T value)
        {
            if (this.enable)
            {
                if (cache.Get(key) != null)
                {
                    cache.Replace(key, value);
                }
                else
                {
                    cache.Add(key, value);
                }
            }
            return;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="absoluteExpiration">过期时间</param>
        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            if (this.enable)
            {
                if (cache.Get(key) != null)
                {
                    cache.Replace(key, value, absoluteExpiration);
                }
                else
                {
                    cache.Add(key, value, absoluteExpiration);
                }
            }
            return;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="slidingExpiration">保存时间</param>
        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (this.enable)
            {
                if (cache.Get(key) != null)
                {
                    cache.Replace(key, value, slidingExpiration);
                }
                else
                {
                    cache.Add(key, value, slidingExpiration);
                }
            }
            return;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="minutes">保存时间(分钟)</param>
        public void Add<T>(string key, T value, int minutes)
        {
             Add(key, value, DateTime.Now.AddMinutes(minutes));
             return;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="priority">优先级</param>
        /// <param name="slidingExpiration">保存时间</param>
        public void Add<T>(string key, T value, CachePriority priority, TimeSpan slidingExpiration)
        {
            Add(key, value, slidingExpiration);
            return;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="priority">优先级</param>
        /// <param name="absoluteExpiration">过期时间</param>
        public void Add<T>(string key, T value, CachePriority priority, DateTime absoluteExpiration)
        {
            Add(key, value, absoluteExpiration);
            return;
        }

        /// <summary>
        /// 移除键中某关键字的缓存并返回相应的值
        /// </summary>
        /// <param name="key">关键字</param>
        public void Remove(string key)
        {
            object result = null;
            if (this.enable && this.Contains(key))
            {
                if (this.Contains(key))
                {
                    result = cache.Get(key);
                    cache.Delete(key);
                }
            }
            return;
        }

        /// <summary>
        /// 移除键中带某关键字的缓存
        /// </summary>
        /// <param name="key">关键字</param>
        public int RemoveContains(string key)
        {
            int result = 0;
            if (this.enable)
            {
                ///   throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// 移除键中以某关键字开头的缓存
        /// </summary>
        /// <param name="key">关键字</param>
        public int RemoveStartWith(string key)
        {
            int result = 0;
            if (this.enable)
            {
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// 移除键中以某关键字结尾的缓存
        /// </summary>
        /// <param name="key">关键字</param>
        public int RemoveEndWith(string key)
        {
            int result = 0;
            if (this.enable)
            {
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// 移除键中所有的缓存
        /// </summary>
        /// <returns>返回缓存中项的数量</returns>
        public int Clear()
        {
            int count = 0;
            if (this.enable)
            {
                cache.FlushAll();
            }
            return count;
        }

        private List<string> keys = new List<string>();

        /// <summary>
        /// 缓存中所有的键列表
        /// </summary>
        public ReadOnlyCollection<string> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}

