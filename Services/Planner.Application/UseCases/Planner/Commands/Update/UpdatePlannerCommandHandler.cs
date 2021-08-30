using BuildingBlocks.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Planner.Commands.Update
{
    public class UpdatePlannerCommandHandler : IRequestHandler<UpdatePlannerCommand>
    {
        private readonly IPlannerDbContext _context;

        public UpdatePlannerCommandHandler(IPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePlannerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Planners
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>(
                    request.Id);
            }

            entity.Update(request.Name, request.Description, request.Duration);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}