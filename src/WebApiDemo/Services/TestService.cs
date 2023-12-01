using Dapper.Lite;
using Utils;

namespace WebApiDemo.Services
{
    /// <summary>
    /// 测试服务
    /// </summary>
    public class TestService : ScopedService
    {
        public string Value { get; set; }

        public TestService()
        {
            Value = "abc";
        }

        public async Task<string> GetValue()
        {
            var session = SF.Get<IDbSession>();
            var client = SF.Get<IDapperLite>();
            Console.WriteLine($"client hashcode={client.GetHashCode()} session hashcode={session.GetHashCode()}");

            var testService = SF.Get<TestService>();
            Console.WriteLine($"testService hashcode={testService.GetHashCode()} value={testService.Value}");

            return await Task.FromResult("测试数据");
        }
    }
}
