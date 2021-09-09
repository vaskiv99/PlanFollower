using BuildingBlocks.Scheduler.Quartz.Interfaces;
using System.Collections.Generic;

namespace BuildingBlocks.Scheduler.Quartz.Models
{
    public abstract class JobCollection
    {
        public abstract IEnumerable<IJobData> EnumerateJobs();
    }
}