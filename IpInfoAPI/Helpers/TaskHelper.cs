using IpInfoAPI.Controllers;

using Quartz;
using Quartz.Impl;

namespace IpInfoAPI.Helpers
{
    public class TaskHelper
    {
        /// <summary>
        /// Schedules update job with given seconds trigger
        /// </summary>
        /// <param name="seconds">Job trigger interval in seconds</param>
        /// <returns></returns>
        public static async Task Init(int seconds)
        {
            StdSchedulerFactory factory = new();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<UpdateIpAddress>()
                .WithIdentity("job", "group")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger", "group")
                .StartNow()
                .WithSimpleSchedule(s => s
                    .WithIntervalInSeconds(seconds)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>
        /// Updates the IP Addresses in the cache
        /// </summary>
        public class UpdateIpAddress : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine("TASK START");
                if (CountryController._memoryCache != null)
                {
                    Console.WriteLine("CACHE INIT");
                    var h = new IpAddressHelper(CountryController._memoryCache);
                    await h.UpdateIpAddresses();
                }
            }
        }
    }
}
