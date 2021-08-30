using BuildingBlocks.Common.Interfaces;
using MediatR;
using Planner.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Planner.Commands.Create
{
    public class CreatePlannerCommandHandler : IRequestHandler<CreatePlannerCommand, Guid>
    {
        private readonly IPlannerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreatePlannerCommandHandler(
            IPlannerDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreatePlannerCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var entity = new Domain.AggregatesModel.PlannerAggregate.Entities.Planner(request.Name, request.Description,
                request.Duration, userId);

            await _context.Planners.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}