using BuildingBlocks.Domain.Events.Abstractions;
using Planner.Domain.Enum;
using Planner.Domain.ValueObjects;
using System;

namespace Planner.Domain.AggregatesModel.PlannerAggregate.Events
{
    public class PlannerCreatedEvent : IDomainEvent
    {
        public PlannerCreatedEvent(
            Guid id,
            string name,
            string description,
            Duration duration,
            Guid userId,
            DateTime createdAt,
            PlannerStatus status)
        {
            Id = id;
            Name = name;
            Description = description;
            Duration = duration;
            UserId = userId;
            CreatedAt = createdAt;
            Status = status;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public Duration Duration { get; }

        public Guid UserId { get; }

        public DateTime CreatedAt { get; }

        public PlannerStatus Status { get; }
    }
}