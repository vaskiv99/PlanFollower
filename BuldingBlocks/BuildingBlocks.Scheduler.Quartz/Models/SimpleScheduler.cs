using System;

namespace BuildingBlocks.Scheduler.Quartz.Models
{
    public class SimpleScheduler
    {
        public DateTimeOffset StartAt { get; set; }

        public DateTimeOffset? EndAt { get; set; }

        public TimeSpan Interval { get; set; }

        public int? RepeatCount { get; set; }
    }
}