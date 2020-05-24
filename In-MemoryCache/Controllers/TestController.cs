using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace In_MemoryCache.Controllers
{
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMemoryCache _mCache;
        public TestController(IMemoryCache mCache)
        {
            _mCache = mCache;
        }

        [HttpGet]
        [Route("set/{key}")]
        public IActionResult SetCache(string key)
        {
            _mCache.Set(key, getEmployee(),
                new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(10))  //Expires in 1 hourse
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))  // If active then reset to another 1 hour
                );
            return Ok(true);
        }

        [HttpGet]
        [Route("get/{key}")]
        public IActionResult GetCache(string key)
        {
           var emp1 =  _mCache.Get<List<Employee>>(key);
            return Ok(emp1);
        }
        private List<Employee> getEmployee()
        {
            return new List<Employee> {
                new Employee {Id =1, FirstName ="Newman", LastName="Croos"},
                new Employee { Id =2 , FirstName = "Nithin", LastName ="Croos"}
            };
        }
    }
}
