using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.AggregatesModel.GoalAggregate.Entities;

namespace Planner.Persistent.EntityConfiguration
{
    public class GoalConfiguration : IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder.ToTable("planner_goals");

            builder.Property(x => x.Name)
                .HasMaxLength(255);

            builder.Property(x => x.Frequency)
                .HasMaxLength(255);

            builder.OwnsOne(x => x.Duration, o =>
            {
                o.Property(x => x.Start).HasColumnName("duration_start");
                o.Property(x => x.End).HasColumnName("duration_end");
            });

            builder.HasOne<Domain.AggregatesModel.PlannerAggregate.Entities.Planner>()
                .WithMany()
                .HasForeignKey(x => x.PlannerId);

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.GoalId);

            builder.HasMany(x => x.Reports)
                .WithOne()
                .HasForeignKey(x => x.GoalId);
        }
    }
}