using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;

namespace Snowball.JobScheduler.Extensions
{
    public static class AddConfiguredQuartzJobTriggersExtension
    {
        public static void AddConfiguredQuartzJobTriggers(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config)
        {
            IEnumerable<CronJobConfig> configuredCronJobs = config.GetSection("CronJobs").Get<IEnumerable<CronJobConfig>>();
            foreach (CronJobConfig cronJob in configuredCronJobs)
            {
                var loggingJobKey = new JobKey(cronJob.JobKey);
                quartz.AddJob(Type.GetType(cronJob.JobClass), loggingJobKey);
                quartz.AddTrigger(options =>
                {
                    options.ForJob(loggingJobKey)
                        .WithIdentity(loggingJobKey + "Trigger")
                        .WithCronSchedule(cronJob.CronExpression);
                });
            }
        }

        public class CronJobConfig
        {
            public string JobKey { get; set; }
            public string JobClass { get; set; }
            public string CronExpression { get; set; }
        }
    }
}
