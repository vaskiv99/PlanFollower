using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Domain.Aggregate;
using BuildingBlocks.Domain.Entity.Implementation;
using Planner.Domain.AggregatesModel.PlannerAggregate.Events;
using Planner.Domain.Enum;
using Planner.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Planner.Domain.AggregatesModel.PlannerAggregate.Entities
{
    public class Planner : Entity, IAggregateRoot
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public Duration Duration { get; private set; }

        public Guid UserId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private readonly List<PlannerStatusItem> _items = new();

        public IReadOnlyCollection<PlannerStatusItem> Items => _items;

        public PlannerStatus CurrentStatus { get; private set; }

        public Planner() { }

        public Planner(string name, string description, Duration duration, Guid userId)
        {
            var createdAt = DateTime.UtcNow;
            var status = duration.Start <= createdAt ? PlannerStatus.InProgress : PlannerStatus.PendingStart;

            var @event = new PlannerCreatedEvent(Guid.NewGuid(), name, description, duration, userId, createdAt, status);

            Enqueue(@event);
            Apply(@event);
        }

        public void ProgressPlanner(string description = null, bool isManualSet = true)
        {
            if (CurrentStatus is not (PlannerStatus.PendingStart or PlannerStatus.Postponed))
            {
                throw new DomainException(
                    $"Is not possible to set the planner status to {PlannerStatus.InProgress} from {CurrentStatus}.");
            }

            if (!isManualSet && CurrentStatus == PlannerStatus.Postponed)
            {
                return;
            }

            var @event = new PlannerStatusItemCreatedEvent(
                PlannerStatus.InProgress,
                description ?? "Planner started",
                DateTime.UtcNow
            );

            Enqueue(@event);
            Apply(@event);
        }

        public void PostponePlanner(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentNullException(nameof(reason), $"{nameof(reason)} can't be empty.");
            }

            if (CurrentStatus is not (PlannerStatus.InProgress or PlannerStatus.PendingStart))
            {
                throw new DomainException(
                    $"Is not possible to set the planner status to {PlannerStatus.Postponed} from {CurrentStatus}.");
            }

            var @event = new PlannerStatusItemCreatedEvent(
                PlannerStatus.Postponed,
                reason,
                DateTime.UtcNow
            );

            Enqueue(@event);
            Apply(@event);
        }

        public void StopPlanner(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentNullException(nameof(reason), $"{nameof(reason)} can't be empty.");
            }

            if (CurrentStatus is PlannerStatus.Completed or PlannerStatus.Stopped)
            {
                throw new DomainException(
                    $"Is not possible to set the planner status to {PlannerStatus.Stopped} from {CurrentStatus}.");
            }

            var @event = new PlannerStatusItemCreatedEvent(
                PlannerStatus.Stopped,
                reason,
                DateTime.UtcNow
            );

            Enqueue(@event);
            Apply(@event);
        }

        public void CompletePlanner()
        {
            var @event = new PlannerStatusItemCreatedEvent(
                PlannerStatus.Completed,
                "The planner's end date is expired. Planner completed.",
                DateTime.UtcNow
            );

            Enqueue(@event);
            Apply(@event);
        }

        public void Update(string name, string description, Duration duration = null)
        {
            Name = name;
            Description = description;

            if (duration is null)
            {
                return;
            }

            if (CurrentStatus == PlannerStatus.Stopped || CurrentStatus == PlannerStatus.Completed)
            {
                throw new DomainException(
                    "Update planner duration is unavailable when planner is completed or stopped");
            }

            var isStartDateChanged = duration.Start.Date != Duration.Start.Date;

            if (isStartDateChanged && CurrentStatus != PlannerStatus.PendingStart)
            {
                throw new DomainException("Update planner start date is unavailable when planner is in progress");
            }

            if (duration.End.Date < Duration.End.Date && CurrentStatus is PlannerStatus.Completed or PlannerStatus.Stopped)
            {
                throw new DomainException("Update planner end date is unavailable when planner is in progress");
            }

            Duration = duration;
        }

        #region Event Apply

        public void Apply(PlannerCreatedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Description = @event.Description;
            Duration = @event.Duration;
            UserId = @event.UserId;
            CreatedAt = @event.CreatedAt;

            _items.Add(new PlannerStatusItem(@event.Status, "Planner initialized", CreatedAt));
        }

        public void Apply(PlannerStatusItemCreatedEvent @event)
        {
            _items.Add(new PlannerStatusItem(@event.Status, @event.Description, @event.Date));

            CurrentStatus = @event.Status;
        }

        #endregion
    }
}