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
    public class DBTestService : ScopedService
    {
        private IDbSession _dbSession;
        private IDbSession<SecondDbFlag> _secondDbSession;
        private IDapperLite _db;
        private IDapperLite<SecondDbFlag> _secondDb;

        public DBTestService(IDbSession dbSession, IDbSession<SecondDbFlag> secondDbSession, IDapperLite db, IDapperLite<SecondDbFlag> secondDb)
        {
            _dbSession = dbSession;
            _secondDbSession = secondDbSession;
            _db = db;
            _secondDb = secondDb;
        }

        /// <summary>
        /// 测试数据库接口
        /// </summary>
        public async Task<List<SysUser>> Test()
        {
            return await _db.GetSession()
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

        /// <summary>
        /// 测试数据库接口
        /// </summary>
        public async Task<List<SysUser>> Test2()
        {
            return await _dbSession
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

        /// <summary>
        /// 测试第二个数据库的接口
        /// </summary>
        public async Task<List<SysUser>> TestSecond()
        {
            return await _secondDb.GetSession()
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

        /// <summary>
        /// 测试第二个数据库的接口
        /// </summary>
        public async Task<List<SysUser>> TestSecond2()
        {
            Console.WriteLine($"hashcode={_secondDbSession.GetHashCode()}");

            return await _secondDbSession
                .Queryable<SysUser>()
                .Where(t => t.Id > 0)
                .ToPageListAsync(1, 100);
        }

    }
}
