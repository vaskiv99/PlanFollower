using BuildingBlocks.Domain.Entity.Implementation;
using Planner.Domain.Enum;
using System;

namespace Planner.Domain.AggregatesModel.PlannerAggregate.Entities
{
    public class PlannerStatusItem : Entity
    {
        public PlannerStatus Status { get; private set; }

        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public Guid PlannerId { get; set; }

        public PlannerStatusItem() { }

        public PlannerStatusItem(PlannerStatus status, string description, DateTime date)
        {
            Status = status;
            Description = description;
            Date = date;
        }
    }
}