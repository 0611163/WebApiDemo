using Dapper.LiteSql;

namespace Porvider
{
    /// <summary>
    /// 第二个数据库DBSession
    /// </summary>
    public class SecondDBSession
    {
        public IDBSession DBSession { get; set; }

        public SecondDBSession(IDBSession dBSession)
        {
            DBSession = dBSession;
        }
    }
}
