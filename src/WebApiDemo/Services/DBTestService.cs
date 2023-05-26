using Dapper.Lite;
using Microsoft.AspNetCore.Mvc;
using Models;
using Porvider;
using Quartz.Impl;
using System.Collections.Specialized;
using Utils;

namespace WebApiDemo.Services
{
    /// <summary>
    /// 数据库测试服务
    /// </summary>
    public class DBTestService : ServiceBase
    {
        private IDbSession _db;
        private IDbSession _secondDB;
        private IDapperLiteClient _dbClient;
        private IDapperLiteClient _secondDbClient;

        public DBTestService(IDbSession db, SecondDBSession secondDB, IDapperLiteClient dbClient, IDapperLiteClient secondDbClient)
        {
            _db = db;
            _secondDB = secondDB.DBSession;
            _dbClient = dbClient;
            _secondDbClient = secondDbClient;
        }

        #region OnStart
        public override async Task OnStart()
        {
            Console.WriteLine("DBTestService 服务启动");
            await Task.CompletedTask;
        }
        #endregion

        #region OnStop
        public override async Task OnStop()
        {
            Console.WriteLine("DBTestService 服务停止");
            await Task.CompletedTask;
        }
        #endregion

        /// <summary>
        /// 测试数据库接口
        /// </summary>
        public async Task<List<SysUser>> Test()
        {
            return await _dbClient.GetSession()
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

        /// <summary>
        /// 测试第二个数据库的接口
        /// </summary>
        public async Task<List<SysUser>> TestSecond()
        {
            return await _secondDbClient.GetSession()
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

    }
}
