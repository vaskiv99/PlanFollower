using BuildingBlocks.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Planner.Application.Common.Interfaces;
using Planner.Domain.AggregatesModel.PlannerAggregate.Events;
using Planner.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Goal.DomainEventHandler
{
    public class PlannerStatusChangedEventHandler : INotificationHandler<PlannerStatusItemCreatedEvent>
    {
        private readonly IPlannerDbContext _context;
        private readonly ILogger<PlannerStatusChangedEventHandler> _logger;

        private readonly
            IDictionary<PlannerStatus, Action<Domain.AggregatesModel.GoalAggregate.Entities.Goal>>
            _updateStatusResolver;

        public PlannerStatusChangedEventHandler(IPlannerDbContext context, ILogger<PlannerStatusChangedEventHandler> logger)
        {
            _context = context;
            _logger = logger;
            _updateStatusResolver =
                new Dictionary<PlannerStatus, Action<Domain.AggregatesModel.GoalAggregate.Entities.Goal>>
                {
                    {PlannerStatus.Postponed, (g) => g.PostponeGoal("The planner was postponed, the goal was automatically postponed too.")},
                    {PlannerStatus.Completed,(g) => g.CompleteGoal("The planner was completed, the goal was automatically completed too.")},
                    {PlannerStatus.Stopped, (g) => g.StopGoal("The planner was stopped, the goal was automatically stopped too.")}
                };
        }

        public async Task Handle(PlannerStatusItemCreatedEvent notification, CancellationToken cancellationToken)
        {
            var goals = await _context.Goals
                .Include(x => x.Items)
                .Where(x => x.PlannerId == notification.PlannerId)
                .ToListAsync(cancellationToken);

            var handler = _updateStatusResolver[notification.Status];

            goals.ForEach(x =>
            {
                try
                {
                    handler(x);
                }
                catch (DomainException ex)
                {
                    _logger.LogError(ex, $"Failed update status for goal: {x.Id}, with status: {x.Status}");
                }
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}