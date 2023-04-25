using Dapper.LiteSql;
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
        private IDBSession _db;
        private IDBSession _secondDB;

        public DBTestService(IDBSession db, SecondDBSession oracleDB)
        {
            _db = db;
            _secondDB = oracleDB.DBSession;
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
            return await _db
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

        /// <summary>
        /// 测试第二个数据库的接口
        /// </summary>
        public async Task<List<SysUser>> TestSecond()
        {
            return await _secondDB
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

    }
}
