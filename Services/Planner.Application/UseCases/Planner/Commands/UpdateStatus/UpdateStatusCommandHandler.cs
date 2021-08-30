using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Domain.Events.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using Planner.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Planner.Commands.UpdateStatus
{
    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand>
    {
        private readonly IPlannerDbContext _context;
        private readonly IEventBus _eventBus;

        private readonly
            IDictionary<PlannerStatus, Action<Domain.AggregatesModel.PlannerAggregate.Entities.Planner, string>>
            _updateStatusResolver;

        public UpdateStatusCommandHandler(IPlannerDbContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
            _updateStatusResolver =
                new Dictionary<PlannerStatus, Action<Domain.AggregatesModel.PlannerAggregate.Entities.Planner, string>>
                {
                    {PlannerStatus.InProgress, (p, d) => p.ProgressPlanner(d)},
                    {PlannerStatus.Postponed, (p, d) => p.PostponePlanner(d)},
                    {PlannerStatus.Stopped, (p, d) => p.StopPlanner(d)}
                };
        }

        public async Task<Unit> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Planners
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>(
                    request.Id);
            }

            if (!_updateStatusResolver.ContainsKey(request.Status))
                throw new DomainInvalidOperation($"Update entity to status:'{request.Status}' doesn't support");

            var resolver = _updateStatusResolver[request.Status];

            resolver(entity, request.Reason);

            await _context.SaveChangesAsync(cancellationToken);

            var events = entity.DequeueUncommittedEvents();
            await _eventBus.PublishAsync(events, cancellationToken);

            return Unit.Value;
        }
    }
}