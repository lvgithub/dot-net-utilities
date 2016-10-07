using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections.ObjectModel;

namespace Core.Web.CacheManage
{
    /// <summary>
    /// 网站缓存管理类
    /// </summary>
    public class WebCache : ICache
    {
        private static readonly object lockObj = new object();
        /// <summary>
        /// 当前的缓存是否可用
        /// </summary>
        private bool enable = false;
        /// <summary>
        /// 默认实例
        /// </summary>
        private static WebCache instance = null;
        /// <summary>
        /// 返回默认WebCache缓存实例
        /// </summary>
        /// <param name="enable">是否可用最好放到配置项里配置下</param>
        public static WebCache GetCacheService(bool enable)
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new WebCache(enable);
                    }
                }
            }
            return instance;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        private WebCache(bool enable)
        {
            this.enable = enable;
        }
        /// <summary>
        /// 获取一个值,指示当前的缓存是否可用
        /// </summary>
        public bool EnableCache
        {
            get
            {
                return this.enable;
            }
        }
        /// <summary>
        /// 获取缓存的类型
        /// </summary>
        public CacheType Type
        {
            get
            {
                return CacheType.Web;
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
                return HttpRuntime.Cache[key] != null;
            }
            return false;
        }
        /// <summary>
        /// 检查系统中是否存在指定的缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns>返回这个类型的值是否存在</returns>
        public bool Contains<T>(string key)
        {
            object value = HttpRuntime.Cache[key];
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
                return (T)HttpRuntime.Cache[key];
            }
            return default(T);
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
                    return HttpRuntime.Cache.Count;
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
                HttpRuntime.Cache.Insert(key, value);
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
                HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
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
                HttpRuntime.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration);
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
            if (this.enable)
            {
                HttpRuntime.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, minutes, 0));
            }
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
            if (this.enable)
            {
                HttpRuntime.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriorityConvert(priority), null);
            }
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
            if (this.enable)
            {
                HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriorityConvert(priority), null);
            }
            return;
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
            object temp = HttpRuntime.Cache[key];
            if (temp != null && temp is T)
            {
                value = (T)temp;
                return true;
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 移除键中某关键字的缓存并返回相应的值
        /// </summary>
        /// <param name="key">关键字</param>
        public void Remove(string key)
        {
            object result = null;
            if (this.enable)
            {
                if (HttpRuntime.Cache[key] != null)
                {
                    result = HttpRuntime.Cache.Remove(key);
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
                System.Collections.IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    if (CacheEnum.Key.ToString().Contains(key))
                    {
                        HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
                        result++;
                    }
                }
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
                System.Collections.IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    if (CacheEnum.Key.ToString().StartsWith(key))
                    {
                        HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
                        result++;
                    }
                }
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
                System.Collections.IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    if (CacheEnum.Key.ToString().EndsWith(key))
                    {
                        HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
                        result++;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 移除键中所有的缓存
        /// </summary>
        public int Clear()
        {
            int result = 0;
            if (this.enable)
            {
                System.Collections.IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
                    result++;
                }
                keys.Clear();
            }
            return result;
        }
        private List<string> keys = new List<string>();
        /// <summary>
        /// 缓存中所有的键列表
        /// </summary>
        public ReadOnlyCollection<string> Keys
        {
            get
            {
                if (this.enable)
                {
                    lock (keys)
                    {
                        keys.Clear();
                        System.Collections.IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
                        while (CacheEnum.MoveNext())
                        {
                            keys.Add(CacheEnum.Key.ToString());
                        }
                    }
                }
                return new ReadOnlyCollection<string>(keys);
            }
        }
        /// <summary>
        /// 对缓存优先级做一个默认的转换
        /// </summary>
        /// <param name="priority">原始的优先级</param>
        /// <returns>目标优先级</returns>
        private CacheItemPriority CacheItemPriorityConvert(CachePriority priority)
        {
            CacheItemPriority p = CacheItemPriority.Default;
            switch (priority)
            {
                case CachePriority.Low:
                    {
                        p = CacheItemPriority.Low;
                        break;
                    }
                case CachePriority.Normal:
                    {
                        p = CacheItemPriority.Normal;
                        break;
                    }
                case CachePriority.High:
                    {
                        p = CacheItemPriority.High;
                        break;
                    }
                case CachePriority.NotRemovable:
                    {
                        p = CacheItemPriority.NotRemovable;
                        break;
                    }
            }
            return p;
        }
    }
}

