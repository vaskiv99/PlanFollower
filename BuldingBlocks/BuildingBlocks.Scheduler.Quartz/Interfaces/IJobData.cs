using BuildingBlocks.Scheduler.Quartz.Models;
using System;

namespace BuildingBlocks.Scheduler.Quartz.Interfaces
{
    public interface IJobData
    {
        string Name { get; set; }
        bool Execute { get; set; }
        Type Type { get; set; }
        SimpleScheduler Trigger { get; set; }
    }
}