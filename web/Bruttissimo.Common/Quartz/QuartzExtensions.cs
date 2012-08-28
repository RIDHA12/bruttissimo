using System;
using Quartz;

namespace Bruttissimo.Common
{
    public static class QuartzExtensions
    {
        /// <summary>
        /// Schedules a job of the provided type to run immediatly.
        /// </summary>
        public static DateTimeOffset StartJob(this IScheduler scheduler, Type jobType)
        {
            IJobDetail detail = JobBuilder.Create(jobType).Build();
            ITrigger trigger = TriggerBuilder.Create().StartNow().Build();
            return scheduler.ScheduleJob(detail, trigger);
        }

        /// <summary>
        /// Schedules a job of the provided type to run using the provided trigger.
        /// </summary>
        public static DateTimeOffset ScheduleJob(this IScheduler scheduler, Type jobType, ITrigger trigger)
        {
            IJobDetail detail = JobBuilder.Create(jobType).Build();
            return scheduler.ScheduleJob(detail, trigger);
        }
    }
}
