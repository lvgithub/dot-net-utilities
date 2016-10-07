using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Core.Web.CacheManage
{
    /// <summary>
    /// 缓存类接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获取一个值,指示当前的缓存是否可用
        /// </summary>
        bool EnableCache
        {
            get;
        }

        /// <summary>
        /// 获取缓存的类型
        /// </summary>
        CacheType Type
        {
            get;
        }

        /// <summary>
        /// 检查缓存中是否存在指定的键
        /// </summary>
        /// <param name="key">要检查的键</param>
        /// <returns>返回一个值,指示检查的键是否存在</returns>
        bool Contains(string key);


        /// <summary>
        /// 检查系统中是否存在指定的缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns>返回这个类型的值是否存在</returns>
        bool Contains<T>(string key);


        /// <summary>
        /// 从缓存中获取指定键的值
        /// </summary>
        /// <param name="key">要获取的键</param>
        /// <returns>返回指定键的值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取缓存中键值的数量
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <returns>返回添加的键值</returns>
        void Add<T>(string key, T value);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="absoluteExpiration">过期时间</param>
        /// <returns>返回添加的键值</returns>
        void Add<T>(string key, T value, DateTime absoluteExpiration);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="slidingExpiration">保存时间</param>
        /// <returns>返回添加的键值</returns>
        void Add<T>(string key, T value, TimeSpan slidingExpiration);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="minutes">保存时间(分钟)</param>
        /// <returns>返回添加的键值</returns>
        void Add<T>(string key, T value, int minutes);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="priority">优先级</param>
        /// <param name="slidingExpiration">保存时间</param>
        void Add<T>(string key, T value, CachePriority priority, TimeSpan slidingExpiration);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">缓存值</param>
        /// <param name="priority">优先级</param>
        /// <param name="absoluteExpiration">过期时间</param>
        void Add<T>(string key, T value, CachePriority priority, DateTime absoluteExpiration);


        /// <summary>
        /// 尝试返回指定的缓存
        /// </summary>
        /// <typeparam name="T">缓存内容的类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <param name="value">缓存的内容</param>
        /// <returns>是否存在这个缓存</returns>
        bool TryGetValue<T>(string key, out T value);

       


        /// <summary>
        /// 移除键中某关键字的缓存并返回相应的值
        /// </summary>
        /// <param name="key">关键字</param>
        void Remove(string key);

        /// <summary>
        /// 移除键中带某关键字的缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>返回被移除的项的数量</returns>
        int RemoveContains(string key);

        /// <summary>
        /// 移除键中以某关键字开头的缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>返回被移除的项的数量</returns>
        int RemoveStartWith(string key);

        /// <summary>
        /// 移除键中以某关键字结尾的缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>返回被移除的项的数量</returns>
        int RemoveEndWith(string key);

        /// <summary>
        /// 移除键中所有的缓存
        /// </summary>
        /// <returns>返回被移除的项的数量</returns>
        int Clear();

        /// <summary>
        /// 缓存中所有的键列表
        /// </summary>
        ReadOnlyCollection<string> Keys
        {
            get;
        }
    }

    /// <summary>
    /// 缓存的优先级
    /// </summary>
    public enum CachePriority
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 低
        /// </summary>
        Low = 1,

        /// <summary>
        /// 普通
        /// </summary>
        Normal = 2,

        /// <summary>
        /// 高
        /// </summary>
        High = 3,

        /// <summary>
        /// 不能被删除
        /// </summary>
        NotRemovable = 4,
    }
}

