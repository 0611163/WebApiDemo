using Dapper.Lite;

namespace Porvider
{
    /// <summary>
    /// 第二个数据库DBSession
    /// </summary>
    public class SecondDBSession
    {
        public IDbSession DBSession { get; set; }

        public SecondDBSession(IDbSession dBSession)
        {
            DBSession = dBSession;
        }
    }
}
