using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.AggregatesModel.GoalAggregate.Entities;

namespace Planner.Persistent.EntityConfiguration
{
    public class GoalStatusItemConfiguration : IEntityTypeConfiguration<GoalStatusItem>
    {
        public void Configure(EntityTypeBuilder<GoalStatusItem> builder)
        {
            builder.ToTable("goal_status_items");
        }
    }
}