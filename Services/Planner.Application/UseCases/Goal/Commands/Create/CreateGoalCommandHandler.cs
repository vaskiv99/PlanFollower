using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Goal.Commands.Create
{
    public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, Guid>
    {
        private readonly IPlannerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateGoalCommandHandler(IPlannerDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var planner = await _context.Planners
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.PlannerId && x.UserId == userId, cancellationToken);

            if (planner is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>(request.PlannerId);
            }

            if (!planner.Duration.IsSubDuration(request.Duration))
            {
                throw new DomainException("Goal duration does not compatible with planner duration");
            }

            var entity = new Domain.AggregatesModel.GoalAggregate.Entities.Goal(request.Name, request.Description,
                request.Duration, request.PlannerId, request.Frequency, request.TrackingType, request.EqualType,
                request.AbstractGoalValue);

            await _context.Goals.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}