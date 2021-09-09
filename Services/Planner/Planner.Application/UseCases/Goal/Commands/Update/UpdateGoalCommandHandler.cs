using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Goal.Commands.Update
{
    public class UpdateGoalCommandHandler : IRequestHandler<UpdateGoalCommand>
    {
        private readonly IPlannerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateGoalCommandHandler(IPlannerDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
        {
            var goal = await _context.Goals
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (goal is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.GoalAggregate.Entities.Goal>(request.Id);
            }

            var userId = _currentUserService.GetUserId();
            var planner = await _context.Planners.FirstOrDefaultAsync(x => x.Id == goal.PlannerId && x.UserId == userId,
                cancellationToken);

            if (planner is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.GoalAggregate.Entities.Goal>(request.Id);
            }

            if (!planner.Duration.IsSubDuration(request.Duration))
            {
                throw new DomainException("Goal duration does not compatible with planner duration");
            }

            goal.Update(request.Name, request.Description, request.Duration);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}