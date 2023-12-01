using Microsoft.Extensions.Logging.Console;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using Utils;

namespace WebApiDemo.Tasks
{
    /// <summary>
    /// 定时任务管理
    /// </summary>
    public class ScheduleJobs : SingletonService
    {
        #region 变量
        private IScheduler _scheduler;
        #endregion

        #region OnStart
        public override async Task OnStart()
        {
            try
            {
                NameValueCollection options = new NameValueCollection();
                string schedulerName = "DefaultQuartzScheduler";
                options.Add("quartz.scheduler.instanceName", schedulerName);
                StdSchedulerFactory schedulerFactory = new StdSchedulerFactory(options);
                _scheduler = await schedulerFactory.GetScheduler(schedulerName);
                if (_scheduler == null)
                {
                    _scheduler = await schedulerFactory.GetScheduler();
                }
                await _scheduler.Start();
                AddJobs(_scheduler);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region OnStop
        public override async Task OnStop()
        {
            await _scheduler.Shutdown();
        }
        #endregion

        #region ScheduleJob
        private async Task ScheduleJob<T>(IScheduler scheduler, string cronString) where T : IJob
        {
            IJobDetail jobDetail = JobBuilder.Create<T>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cronString).Build();
            await scheduler.ScheduleJob(jobDetail, trigger);
        }
        #endregion

        private async void AddJobs(IScheduler scheduler)
        {
            await ScheduleJob<TestJob>(scheduler, "0/30 * * * * ?");
        }

    }
}
