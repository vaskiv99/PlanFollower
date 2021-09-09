using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.NameTranslation;
using Planner.Application.Common.Interfaces;
using Planner.Domain.AggregatesModel.GoalAggregate.Entities;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using Planner.Domain.AggregatesModel.PlannerAggregate.Entities;
using Planner.Domain.Enum;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Persistent
{
    public class PlannerDbContext : DbContext, IPlannerDbContext
    {
        #region Constructors

        public PlannerDbContext(DbContextOptions<PlannerDbContext> options)
            : base(options)
        {
        }

        static PlannerDbContext()
        {
            RegisterTypes();
        }

        #endregion

        #region Tables

        public DbSet<Domain.AggregatesModel.PlannerAggregate.Entities.Planner> Planners { get; set; }
        public DbSet<PlannerStatusItem> PlannerStatusItems { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalStatusItem> GoalStatusItems { get; set; }
        public DbSet<Report> Reports { get; set; }

        #endregion

        #region DbContext members

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.HasPostgresEnum<PlannerStatus>(nameTranslator: new NpgsqlSnakeCaseNameTranslator());
            builder.HasPostgresEnum<EqualType>(nameTranslator: new NpgsqlSnakeCaseNameTranslator());
            builder.HasPostgresEnum<TimePeriod>(nameTranslator: new NpgsqlSnakeCaseNameTranslator());
            builder.HasPostgresEnum<TrackingType>(nameTranslator: new NpgsqlSnakeCaseNameTranslator());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var count = await base.SaveChangesAsync(cancellationToken);

            return count;
        }

        #endregion

        #region Methods

        public static void RegisterTypes()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PlannerStatus>("planner_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EqualType>("equal_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TimePeriod>("time_period");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TrackingType>("tracking_type");
        }

        #endregion
    }
}