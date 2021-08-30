using BuildingBlocks.Common.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Planner.Queries.Get
{
    public class GetPlannerQueryHandler : IRequestHandler<GetPlannerQuery, PlannerView>
    {
        private readonly IPlannerDbContext _context;

        public GetPlannerQueryHandler(IPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<PlannerView> Handle(GetPlannerQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Planners
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>(
                    request.Id);
            }

            return entity.Adapt<PlannerView>();
        }
    }
}