using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Planner.Domain.AggregatesModel.GoalAggregate.Entities;
using Planner.Domain.AggregatesModel.PlannerAggregate.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Application.Common.Interfaces
{
    /// <summary>
    /// Context to database.
    /// </summary>
    public interface IPlannerDbContext
    {
        DbSet<Domain.AggregatesModel.PlannerAggregate.Entities.Planner> Planners { get; set; }

        DbSet<PlannerStatusItem> PlannerStatusItems { get; set; }

        DbSet<Goal> Goals { get; set; }

        DbSet<GoalStatusItem> GoalStatusItems { get; set; }

        DbSet<Report> Reports { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DatabaseFacade Database { get; }
    }
}