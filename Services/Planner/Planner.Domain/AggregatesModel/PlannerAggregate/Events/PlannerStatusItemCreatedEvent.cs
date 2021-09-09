using BuildingBlocks.Domain.Events.Abstractions;
using Planner.Domain.Enum;
using System;

namespace Planner.Domain.AggregatesModel.PlannerAggregate.Events
{
    public class PlannerStatusItemCreatedEvent : IDomainEvent
    {
        public PlannerStatusItemCreatedEvent(PlannerStatus status, string description, DateTime date, Guid plannerId)
        {
            Status = status;
            Description = description;
            Date = date;
            PlannerId = plannerId;
        }

        public PlannerStatus Status { get; }

        public string Description { get; }

        public DateTime Date { get; }

        public Guid PlannerId { get; set; }
    }
}