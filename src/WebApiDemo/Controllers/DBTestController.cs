using Dapper.Lite;
using Microsoft.AspNetCore.Mvc;
using Models;
using Porvider;
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

        public DBTestController(DBTestService dbTestService)
        {
            _dbTestService = dbTestService;
        }

        /// <summary>
        /// 测试数据库接口
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<List<SysUser>> Test()
        {
            return await _dbTestService.Test();
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
    }
}
