using BuildingBlocks.Scheduler.Quartz.Interfaces;
using System;

namespace BuildingBlocks.Scheduler.Quartz.Models
{
    public class JobData : IJobData
    {
        public string Name { get; set; }

        public bool Execute { get; set; }

        public Type Type { get; set; }

        public SimpleScheduler Trigger { get; set; }
    }
}