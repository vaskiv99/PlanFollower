using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Common.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Goal.Queries.Get
{
    public class GetGoalByIdQueryHandler : IRequestHandler<GetGoalByIdQuery, ExtendedGoalView>
    {
        private readonly IPlannerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetGoalByIdQueryHandler(IPlannerDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<ExtendedGoalView> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
        {
            var goal = await _context.Goals
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (goal is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.GoalAggregate.Entities.Goal>(request.Id);
            }

            var userId = _currentUserService.GetUserId();
            var isPlannerExist = await _context.Planners.AnyAsync(x => x.Id == goal.PlannerId && x.UserId == userId,
                cancellationToken);

            if (!isPlannerExist)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.GoalAggregate.Entities.Goal>(request.Id);
            }

            return goal.Adapt<ExtendedGoalView>();
        }
    }
}