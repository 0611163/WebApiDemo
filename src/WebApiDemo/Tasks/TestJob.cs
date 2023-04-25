using Quartz;
using System.Diagnostics;
using Utils;

namespace WebApiDemo.Tasks
{
    /// <summary>
    /// 定时任务示例
    /// </summary>
    [DisallowConcurrentExecution]
    public class TestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 执行定时任务");
            await Task.CompletedTask;
        }
    }
}
