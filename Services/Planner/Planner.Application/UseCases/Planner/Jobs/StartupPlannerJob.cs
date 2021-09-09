using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Planner.Application.Common.Interfaces;
using Planner.Domain.Enum;
using Polly;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Planner.Jobs
{
    public class StartupPlannerJob : IJob
    {
        private readonly IPlannerDbContext _context;
        private readonly ILogger<StartupPlannerJob> _logger;

        public StartupPlannerJob(IPlannerDbContext context, ILogger<StartupPlannerJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("___ Job for starting up planners started ___");

            var hasPlannersToProgress = true;
            var skip = 0;
            const int take = 100;

            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) => { _logger.LogCritical(ex, "___ Failed operation of starting up planners ___"); });

            while (hasPlannersToProgress)
            {
                await policy.Execute(async () =>
                {
                    var plannersToProgress = await _context.Planners
                        .Where(x => x.Duration.Start.Date <= DateTime.UtcNow.Date)
                        .Where(x => x.CurrentStatus == PlannerStatus.PendingStart)
                        .OrderBy(x => x.Duration.Start)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync(context.CancellationToken);

                    hasPlannersToProgress = plannersToProgress.Any();

                    if (!hasPlannersToProgress)
                    {
                        return;
                    }

                    plannersToProgress.ForEach(x => x.ProgressPlanner("Planner started automatically", false));

                    await _context.SaveChangesAsync(context.CancellationToken);

                    _logger.LogInformation($"___ Started up [{plannersToProgress.Count}] planners ___");

                    skip += take;
                });
            }

            _logger.LogInformation("___ Job for starting up planners completed ___");
        }
    }
}