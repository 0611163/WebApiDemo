using Utils;

namespace WebApiDemo.Services
{
    /// <summary>
    /// 测试服务
    /// </summary>
    public class TestService : ServiceBase
    {
        public async Task<string> GetValue()
        {
            return await Task.FromResult("测试数据");
        }
    }
}
