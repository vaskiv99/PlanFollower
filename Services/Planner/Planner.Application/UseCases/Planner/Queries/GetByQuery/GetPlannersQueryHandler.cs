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

namespace Planner.Application.UseCases.Planner.Queries.GetByQuery
{
    public class GetPlannersQueryHandler : IRequestHandler<GetPlannersQuery, QueryResult<PlannerView>>
    {
        private readonly IPlannerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetPlannersQueryHandler(IPlannerDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<QueryResult<PlannerView>> Handle(GetPlannersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var baseQuery = _context.Planners
                .Where(x => x.UserId == userId)
                .WhereIf(request.Statuses != null && request.Statuses.Any(),
                    x => request.Statuses.Contains(x.CurrentStatus));

            var count = await baseQuery.CountAsync(cancellationToken);

            if (count == 0)
            {
                return new QueryResult<PlannerView>();
            }

            var data = await baseQuery.Paginate(request.PageQuery)
                .ToListAsync(cancellationToken);

            var paging = request.PageQuery.CreatePaging(count);
            var result = data.Adapt<List<PlannerView>>();

            return new QueryResult<PlannerView>(result, paging);
        }
    }
}