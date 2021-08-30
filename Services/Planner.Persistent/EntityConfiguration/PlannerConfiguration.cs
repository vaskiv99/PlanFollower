using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Planner.Persistent.EntityConfiguration
{
    public class PlannerConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.PlannerAggregate.Entities.Planner> builder)
        {
            builder.ToTable("planners");

            builder.Property(x => x.Name)
                .HasMaxLength(255);

            builder.OwnsOne(x => x.Duration, o =>
            {
                o.Property(x => x.Start).HasColumnName("duration_start");
                o.Property(x => x.End).HasColumnName("duration_end");
            });

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.PlannerId);
        }
    }
}