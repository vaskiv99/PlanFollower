using BuildingBlocks.Domain.Entity.Implementation;
using Planner.Domain.Enum;
using System;

namespace Planner.Domain.AggregatesModel.GoalAggregate.Entities
{
    public class GoalStatusItem : Entity
    {
        public PlannerStatus Status { get; private set; }

        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public Guid GoalId { get; set; }

        public GoalStatusItem() { }

        public GoalStatusItem(PlannerStatus status, string description, DateTime date)
        {
            Status = status;
            Description = description;
            Date = date;
        }
    }
}