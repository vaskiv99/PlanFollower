using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.AggregatesModel.PlannerAggregate.Entities;

namespace Planner.Persistent.EntityConfiguration
{
    public class PlannerStatusItemConfiguration : IEntityTypeConfiguration<PlannerStatusItem>
    {
        public void Configure(EntityTypeBuilder<PlannerStatusItem> builder)
        {
            builder.ToTable("planner_status_items");
        }
    }
}