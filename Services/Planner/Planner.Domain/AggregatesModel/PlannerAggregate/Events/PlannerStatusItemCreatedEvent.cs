using BuildingBlocks.Domain.Events.Abstractions;
using Planner.Domain.Enum;
using System;

namespace Planner.Domain.AggregatesModel.PlannerAggregate.Events
{
    public class PlannerStatusItemCreatedEvent : IDomainEvent
    {
        public PlannerStatusItemCreatedEvent(PlannerStatus status, string description, DateTime date)
        {
            Status = status;
            Description = description;
            Date = date;
        }

        public PlannerStatus Status { get; }

        public string Description { get; }

        public DateTime Date { get; }
    }
}