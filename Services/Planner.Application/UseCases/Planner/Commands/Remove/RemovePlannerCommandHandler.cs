using BuildingBlocks.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Planner.Commands.Remove
{
    public class RemovePlannerCommandHandler : IRequestHandler<RemovePlannerCommand>
    {
        private readonly IPlannerDbContext _context;

        public RemovePlannerCommandHandler(IPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemovePlannerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Planners
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>(
                    request.Id);
            }

            _context.Planners.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}