using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Planner.Application.Common.Interfaces;
using Planner.Application.UseCases.Planner.Jobs;
using Planner.Domain.Enum;
using Polly;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Goal.Jobs
{
    public class StartupGoalJob : IJob
    {
        private readonly IPlannerDbContext _context;
        private readonly ILogger<StartupPlannerJob> _logger;

        public StartupGoalJob(IPlannerDbContext context, ILogger<StartupPlannerJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("___ Job for starting up goals started ___");

            var hasGoalToProgress = true;
            var skip = 0;
            const int take = 100;

            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) => { _logger.LogCritical(ex, "___ Failed operation of starting up goals ___"); });

            while (hasGoalToProgress)
            {
                await policy.Execute(async () =>
                {
                    var goalsToProgress = await _context.Goals
                        .Where(x => x.Duration.Start.Date <= DateTime.UtcNow.Date)
                        .Where(x => x.Status == PlannerStatus.PendingStart)
                        .OrderBy(x => x.Duration.Start)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync(context.CancellationToken);

                    hasGoalToProgress = goalsToProgress.Any();

                    if (!hasGoalToProgress)
                    {
                        return;
                    }

                    goalsToProgress.ForEach(x => x.ProgressGoal("Goal started automatically", false));

                    await _context.SaveChangesAsync(context.CancellationToken);

                    _logger.LogInformation($"___ Started up [{goalsToProgress.Count}] goals ___");

                    skip += take;
                });
            }

            _logger.LogInformation("___ Job for starting up goals completed ___");
        }
    }
}