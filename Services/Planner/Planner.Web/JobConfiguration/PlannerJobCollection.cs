using BuildingBlocks.Scheduler.Quartz.Interfaces;
using BuildingBlocks.Scheduler.Quartz.Models;
using Planner.Application.UseCases.Goal.Jobs;
using Planner.Application.UseCases.Planner.Jobs;
using System;
using System.Collections.Generic;

namespace Planner.Web.JobConfiguration
{
    public class PlannerJobCollection : JobCollection
    {
        private readonly IJobData _startupPlannerJob = new JobData
        {
            Execute = true,
            Name = "startup-planner",
            Type = typeof(StartupPlannerJob),
            Trigger = new SimpleScheduler
            {
                StartAt = DateTimeOffset.UtcNow.Date.AddMinutes(1),
                Interval = TimeSpan.FromDays(1)
            }
        };

        private readonly IJobData _completePlannerJob = new JobData
        {
            Execute = true,
            Name = "complete-planner",
            Type = typeof(CompletePlannerJob),
            Trigger = new SimpleScheduler
            {
                StartAt = DateTimeOffset.UtcNow.Date.AddMinutes(3),
                Interval = TimeSpan.FromDays(1)
            }
        };

        private readonly IJobData _startupGoalJob = new JobData
        {
            Execute = true,
            Name = "startup-goal",
            Type = typeof(StartupGoalJob),
            Trigger = new SimpleScheduler
            {
                StartAt = DateTimeOffset.UtcNow.Date.AddMinutes(5),
                Interval = TimeSpan.FromDays(1)
            }
        };

        private readonly IJobData _completeGoalJob = new JobData
        {
            Execute = true,
            Name = "complete-goal",
            Type = typeof(CompleteGoalJob),
            Trigger = new SimpleScheduler
            {
                StartAt = DateTimeOffset.UtcNow.Date.AddMinutes(7),
                Interval = TimeSpan.FromDays(1)
            }
        };

        public override IEnumerable<IJobData> EnumerateJobs()
        {
            yield return _startupPlannerJob;
            yield return _startupGoalJob;
            yield return _completePlannerJob;
            yield return _completeGoalJob;
        }
    }
}