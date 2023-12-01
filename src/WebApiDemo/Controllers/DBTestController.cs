using Dapper.Lite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Porvider;
using System.Collections.Generic;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    /// <summary>
    /// 测试数据库接口
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Tags("数据库操作接口")]
    public class DBTestController : ControllerBase
    {
        private DBTestService _dbTestService;

        private IMemoryCache _cache;

        public DBTestController(DBTestService dbTestService, IMemoryCache cache)
        {
            _dbTestService = dbTestService;
            _cache = cache;
        }

        /// <summary>
        /// 测试数据库接口
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<List<SysUser>> Test()
        {
            Console.WriteLine($"cache hashcode={_cache.GetHashCode()}");

            return await _cache.GetOrCreateAsync("cache1", async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(100));

                return await _dbTestService.Test();
            });
        }

        /// <summary>
        /// 测试数据库接口
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<List<SysUser>> Test2()
        {
            return await _dbTestService.Test2();
        }

        /// <summary>
        /// 测试第二个数据库的接口
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<List<SysUser>> TestSecond()
        {
            return await _dbTestService.TestSecond();
        }

        /// <summary>
        /// 测试第二个数据库的接口
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<List<SysUser>> TestSecond2()
        {
            return await _dbTestService.TestSecond2();
        }
    }
}
