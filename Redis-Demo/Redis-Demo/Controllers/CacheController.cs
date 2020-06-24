using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Redis_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly ICacheStringService _cacheService;
        private readonly ICacheService _cacheService1;
        public CacheController(IDatabase database, ICacheStringService cacheService, ICacheService cacheService1)
        {
            _database = database;
            _cacheService = cacheService;
            _cacheService1 = cacheService1;
        }
        [HttpGet]
        public string Get([FromQuery]string key)
        {
            return _database.StringGet(key);
        }

        [HttpGet]
        public async Task<string> GetAsync([FromQuery]string key)
        {
            return await _cacheService.GetCacheStringValueAsync(key);
        }
        // POST: api/Cache
        [HttpPost]
        public void Post([FromBody] KeyValuePair<string,string> keyValue)
        {
            _database.StringSet(keyValue.Key, keyValue.Value);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] KeyValuePair<string, string> keyValue)
        {
            await _cacheService.SetCacheStringValueAsync(keyValue.Key, keyValue.Value);
        }

        [HttpGet]
        [Route("setwithfun")]
        public async Task<string> SetCatchFromFunction(string keyValue)
        {
            return await _cacheService1.GetOrSetCacheAsync<string>(keyValue, () => GetVaue("Test"));
        }

        private async Task<string> GetVaue(string opt)
        {
            return await Task.Run(() => $"MyString - {opt}");
        }
    }
}
