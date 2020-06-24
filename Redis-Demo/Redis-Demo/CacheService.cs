using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Redis_Demo
{
    public interface ICacheService
    {
        Task<T> GetCacheAsync<T>(string key);
        Task<bool> SetCacheAsync<T>(string key, T value, TimeSpan? expiryInterval = null);

        Task<T> GetCacheAsync<T>(CacheRegion region);
        Task<bool> SetCacheAsync<T>(CacheRegion region, T value);

        Task<T> GetOrSetCacheAsync<T>(string key, Func<Task<T>> valueGetterAsync = null);
        Task<T> GetOrSetCacheAsync<T>(CacheRegion region, Func<Task<T>> valueGetterAsync = null);

        Task<bool> RemoveCacheAsyn(string key);
        Task<bool> RemoveCacheAsync(CacheRegion region);
    }
    //for This service we install EasyCaching.Redis Nuget package it support CacheProviderFactory and CacheProvider
    // so We can store and retrive any type of values in the cache
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IDatabase database)
        {
            _database = database;
        }
        public async Task<T> GetCacheAsync<T>(string key)
        {
            var cacheValue = await _database.StringGetAsync(key);
            if (cacheValue.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(cacheValue);
            }
            return default(T);
        }

        public async Task<T> GetCacheAsync<T>(CacheRegion region)
        {
            var cacheValue = await _database.HashGetAsync(region.HashMapName, region.HashKey);
            if (cacheValue.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(cacheValue);
            }
            return default(T);
        }

        public async Task<bool> SetCacheAsync<T>(string key, T value, TimeSpan? expiryInterval = null)
        {
            if (value == null) return false;
            try
            {
                var expiry = expiryInterval ?? TimeSpan.FromHours(2);
                return await _database.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SetCacheAsync<T>(CacheRegion region, T value)
        {
            if (value == null) return false;
            try
            {
                return await _database.HashSetAsync(region.HashMapName, region.HashKey,JsonConvert.SerializeObject(value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<T> GetOrSetCacheAsync<T>(string key, Func<Task<T>> valueGetterAsync = null)
        {
            try
            {
                var cacheValue =await  _database.StringGetAsync(key);
                if (cacheValue.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(cacheValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (valueGetterAsync != null)
            {
                var result = await valueGetterAsync();
                await SetCacheAsync<T>(key, result);
                return result;
            }
            return default(T);
        }

        public async Task<T> GetOrSetCacheAsync<T>(CacheRegion region, Func<Task<T>> valueGetterAsync = null)
        {
            try
            {
                var cacheValue = await _database.HashGetAsync(region.HashMapName, region.HashKey);
                if (cacheValue.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(cacheValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (valueGetterAsync != null)
            {
                var result = await valueGetterAsync();
                await SetCacheAsync<T>(region, result);
                return result;
            }
            return default(T);
        }

        public async Task<bool> RemoveCacheAsyn(string key)
        {
            try
            {
                return !string.IsNullOrWhiteSpace(key) && await _database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> RemoveCacheAsync(CacheRegion region)
        {
            try
            {
                return await _database.HashDeleteAsync(region.HashMapName, region.HashKey);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

//// HashGetAll -- Gets all the items in the hashMap
//var allHash = redis.HashGetAll(hashMapName);
//foreach (var item in allHash)
//{
//Console.WriteLine($"Key: {item.Name}, Value:{item.Value});
//}
 
//// HashValues -- Gets all the values in the hashMap
//var values = redis.HashValues(hashMapName);
//foreach (var value in values)
//{
// Console.WriteLine(value);
//}
 
//// HashKeys -- Gets all the keys in the hashMap
//var keys = redis.HashKeys(hashMapName);
//foreach (var key in keys)
//{
// Console.WriteLine(key);
//}
 
//// HashLength -- Gets the length of the hashMap
//var len = redis.HashLength(hashMapName);  
 
//// HashExists -- Checks whether the key exists in the hashMap or not
//if (redis.HashExists(hashMapName, key))
//{
// var item = redis.HashGet(hashMapName, key);
//}