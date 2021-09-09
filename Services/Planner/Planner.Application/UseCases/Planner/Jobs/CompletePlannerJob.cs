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
    public class CompletePlannerJob : IJob
    {
        private readonly IPlannerDbContext _context;
        private readonly ILogger<StartupPlannerJob> _logger;

        public CompletePlannerJob(IPlannerDbContext context, ILogger<StartupPlannerJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("___ Job for complete planners started ___");

            var hasPlannersToComplete = true;
            var skip = 0;
            const int take = 100;

            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) => { _logger.LogCritical(ex, "___ Failed operation of complete planners ___"); });

            while (hasPlannersToComplete)
            {
                await policy.Execute(async () =>
                {
                    var plannersToComplete = await _context.Planners
                        .Where(x => x.Duration.End.Date <= DateTime.UtcNow.Date)
                        .Where(x => x.CurrentStatus == PlannerStatus.InProgress)
                        .OrderBy(x => x.Duration.End)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync(context.CancellationToken);

                    hasPlannersToComplete = plannersToComplete.Any();

                    if (!hasPlannersToComplete)
                    {
                        return;
                    }

                    plannersToComplete.ForEach(x => x.CompletePlanner());

                    await _context.SaveChangesAsync(context.CancellationToken);

                    _logger.LogInformation($"___ Completed [{plannersToComplete.Count}] planners ___");

                    skip += take;
                });
            }

            _logger.LogInformation("___ Job for complete planners completed ___");
        }
    }
}