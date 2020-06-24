using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis_Demo
{
    public interface ICacheStringService
    {
        Task<string> GetCacheStringValueAsync(string key);
        Task SetCacheStringValueAsync(string key, string value);
    }
    public class RedisCachStringService : ICacheStringService
    {
        private readonly IDatabase _database;
        public RedisCachStringService(IDatabase database)
        {
            _database = database;
        }
        public async Task<string> GetCacheStringValueAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task SetCacheStringValueAsync(string key, string value)
        {
            //var keyValue = new KeyValuePair<string, string>(key, value);
            await _database.StringSetAsync(key, value);
        }
    }
}
