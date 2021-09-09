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
    public class CompleteGoalJob : IJob
    {
        private readonly IPlannerDbContext _context;
        private readonly ILogger<StartupPlannerJob> _logger;

        public CompleteGoalJob(IPlannerDbContext context, ILogger<StartupPlannerJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("___ Job for complete goals started ___");

            var hasGoalsToComplete = true;
            var skip = 0;
            const int take = 100;

            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) => { _logger.LogCritical(ex, "___ Failed operation of complete goals ___"); });

            while (hasGoalsToComplete)
            {
                await policy.Execute(async () =>
                {
                    var goalsToComplete = await _context.Goals
                        .Where(x => x.Duration.End.Date <= DateTime.UtcNow.Date)
                        .Where(x => x.Status == PlannerStatus.InProgress)
                        .OrderBy(x => x.Duration.End)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync(context.CancellationToken);

                    hasGoalsToComplete = goalsToComplete.Any();

                    if (!hasGoalsToComplete)
                    {
                        return;
                    }

                    goalsToComplete.ForEach(x => x.CompleteGoal());

                    await _context.SaveChangesAsync(context.CancellationToken);

                    _logger.LogInformation($"___ Completed [{goalsToComplete.Count}] goals ___");

                    skip += take;
                });
            }

            _logger.LogInformation("___ Job for complete goals completed ___");
        }
    }
}