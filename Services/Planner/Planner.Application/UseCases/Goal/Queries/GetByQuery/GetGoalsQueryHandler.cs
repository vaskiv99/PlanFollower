using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Common.Interfaces;
using BuildingBlocks.Common.Models;
using BuildingBlocks.Common.Utils;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Goal.Queries.GetByQuery
{
    public class GetGoalsQueryHandler : IRequestHandler<GetGoalsQuery, QueryResult<GoalView>>
    {
        private readonly IPlannerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetGoalsQueryHandler(
            IPlannerDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<QueryResult<GoalView>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var isPlannerExist = await _context.Planners.AnyAsync(x => x.Id == request.PlannerId && x.UserId == userId,
                cancellationToken);

            if (!isPlannerExist)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>(request.PlannerId);
            }

            var baseQuery = _context.Goals
                .Where(x => x.PlannerId == request.PlannerId);

            var count = await baseQuery.CountAsync(cancellationToken);

            if (count == 0)
            {
                return new QueryResult<GoalView>();
            }

            var data = await baseQuery
                .Paginate(request.PageQuery)
                .ToListAsync(cancellationToken);

            var response = data.Adapt<List<GoalView>>();

            return new QueryResult<GoalView>(response, request.PageQuery.CreatePaging(count));
        }
    }
}