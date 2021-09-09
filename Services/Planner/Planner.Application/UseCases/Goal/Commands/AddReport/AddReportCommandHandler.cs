using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.UseCases.Goal.Commands.AddReport
{
    public class AddReportCommandHandler : IRequestHandler<AddReportCommand>
    {
        private readonly IPlannerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddReportCommandHandler(IPlannerDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(AddReportCommand request, CancellationToken cancellationToken)
        {
            var goal = await _context.Goals
                .FirstOrDefaultAsync(x => x.Id == request.GoalId, cancellationToken);

            if (goal is null)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.GoalAggregate.Entities.Goal>(request.GoalId);
            }

            var userId = _currentUserService.GetUserId();
            var isPlannerExist = await _context.Planners.AnyAsync(x => x.Id == goal.PlannerId && x.UserId == userId,
                cancellationToken);

            if (!isPlannerExist)
            {
                throw AggregateNotFoundException.For<Domain.AggregatesModel.GoalAggregate.Entities.Goal>(request.GoalId);
            }

            var report = goal.CreateReport(request.Description, request.Date, request.ValueOfProgress);

            await _context.Reports.AddAsync(report, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}