using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Cache
{
    public class Cache
    {
        public static pActionCollection Actions { get; set; } = new pActionCollection();
        public static pItemCollection Items { get; set; } = new pItemCollection();
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static T GetCache<T>(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            var obj = objCache[CacheKey];
            return obj == null ? default(T) : (T)obj;
        }
        public static T GetCache<T>(string CacheKey, Func<T> FDataSource, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            var obj = objCache[CacheKey];
            if (obj == null)
            {
                var data = FDataSource();
                obj = data;
                objCache.Insert(CacheKey, data, null, absoluteExpiration, slidingExpiration);
            }
            return obj == null ? default(T) : (T)obj;
        }

        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            var obj = objCache[CacheKey];
            return obj;
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout, System.Web.Caching.CacheDependency CacheDependencies = null)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, CacheDependencies, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, System.Web.Caching.CacheDependency CacheDependencies = null)//, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, CacheDependencies, absoluteExpiration, TimeSpan.Zero);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveAllCache(string CacheKey)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            System.Collections.IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        public static T GetAbsoluteKeepCache<T>(string CacheKey, Func<T> FDataSource, Int32 CacheKeepSeconds = 60, System.Web.Caching.CacheDependency CacheDependencies = null) where T : new()
        {

            var data = Cache.GetCache<T>(CacheKey);
            if (data == null)
            {
                data = FDataSource();
                if (data == null) data = new T();
                Cache.SetCache(CacheKey, data, DateTime.Now.AddSeconds(CacheKeepSeconds), CacheDependencies);
            }
            return data;
        }
        public static T GetSlidingKeepCache<T>(string CacheKey, Func<T> FDataSource, Int32 CacheKeepSeconds = 60, System.Web.Caching.CacheDependency CacheDependencies = null) where T : new()
        {

            var data = Cache.GetCache<T>(CacheKey);
            if (data == null)
            {
                data = FDataSource();
                if (data == null) data = new T();
                Cache.SetCache(CacheKey, data, new TimeSpan(0, 0, CacheKeepSeconds), CacheDependencies);
            }
            return data;
        }

    }
    public class Cache<T> : pBase
     where T : class, Table.IBase, new()
    {
        public Cache()
        {
        }
        private Cache(Int32 KeyID)
        {
            ID = KeyID;
            Actions = new pActionCollection(KeyID);
            Items = new pItemCollection(KeyID);

            Model = new T();
        }
        public static string CacheKey = $"ENTITY/{typeof(T).ToString()}";

        public T Model { get; set; } = new T();

        public static Cache<T> Init<E>(Int32 KeyID, Int32 CacheKeepSeconds = 600)
            where E : System.Data.Entity.DbContext, new()
        {
            var data = Cache.GetAbsoluteKeepCache($"{CacheKey}/{KeyID}", () =>
              K.Y.DLL.Entity._API<E>.Exec(EF =>
              {
                  var m = new Cache<T>(KeyID);
                  m.Model = EF.Set<T>().FirstOrDefault(p => p.ID == KeyID) ?? new T();
                  return m;
              }, false),
             CacheKeepSeconds);
            return data;
        }
        public static Cache<T> Init<E>(System.Linq.Expressions.Expression<Func<T, Boolean>> FSearch,String CacheSign, Int32 CacheKeepSeconds = 600)
            where E : System.Data.Entity.DbContext, new()
        {
            var data = Cache.GetAbsoluteKeepCache($"{CacheKey}/{CacheSign}", () =>
              K.Y.DLL.Entity._API<E>.Exec(EF =>
              {
                  var Model = EF.Set<T>().FirstOrDefault(FSearch) ?? new T();
                  var m = new Cache<T>(Model.ID);
                  m.Model = Model;
                  return m;
              }, false),
             CacheKeepSeconds);
            return data;
        }
        public static void Refresh(T M, Int32 CacheKeepSeconds = 600)
        {
            var data = Cache.GetAbsoluteKeepCache($"{CacheKey}/{M.ID}", () =>
            {
                var m = new Cache<T>(M.ID);
                m.Model = M;
                return m;
            },
             CacheKeepSeconds);
            data.Model = M;
        }
        public static void Remove(Int32 KeyID)
        {
            Cache.RemoveAllCache($"{CacheKey}/{KeyID}");
        }
    }
}