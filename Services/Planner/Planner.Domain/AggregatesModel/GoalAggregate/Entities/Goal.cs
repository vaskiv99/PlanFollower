using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Domain.Aggregate;
using BuildingBlocks.Domain.Entity.Implementation;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using Planner.Domain.AggregatesModel.GoalAggregate.ValueObjects;
using Planner.Domain.Enum;
using Planner.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Domain.AggregatesModel.GoalAggregate.Entities
{
    public class Goal : Entity, IAggregateRoot
    {
        #region Properties

        /// <summary>
        /// Name of goal
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of goal
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Duration of goal
        /// </summary>
        public Duration Duration { get; private set; }

        /// <summary>
        /// Frequency string of goal (e.g 2_times_per_3_week)
        /// For build this value use <see cref="ValueObjects.Frequency"/>
        /// </summary>
        public string Frequency { get; private set; }

        /// <summary>
        /// Planner identifier
        /// </summary>
        public Guid PlannerId { get; private set; }

        /// <summary>
        ///Abstract goal value it's can be the amount of money that you want to earn.
        ///It's can be anything that can convert to digit.
        /// </summary>
        public decimal? AbstractGoalValue { get; private set; }

        /// <summary>
        /// Progress tracking type
        /// </summary>
        public TrackingType TrackingType { get; private set; }

        /// <summary>
        /// Equal type
        /// </summary>
        public EqualType EqualType { get; private set; }

        /// <summary>
        /// Status of goal
        /// </summary>
        public PlannerStatus Status { get; private set; }

        /// <summary>
        /// Date of goal creation
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Goal status history
        /// </summary>
        private readonly List<GoalStatusItem> _items = new();

        public IReadOnlyCollection<GoalStatusItem> Items => _items;

        /// <summary>
        /// Goal reports history
        /// </summary>
        private readonly List<Report> _reports = new();

        public IReadOnlyCollection<Report> Reports => _reports;

        #endregion

        #region Constructors

        public Goal() { }

        public Goal(string name, string description,
            Duration duration, Guid plannerId,
            Frequency frequency, TrackingType trackingType,
            EqualType equalType, decimal? abstractGoalValue)
        {
            if (trackingType == TrackingType.AbstractGoalValue && !abstractGoalValue.HasValue)
            {
                throw new ArgumentNullException(nameof(abstractGoalValue),
                    $"{nameof(abstractGoalValue)} required when tracking type is {TrackingType.AbstractGoalValue}");
            }

            var createdAt = DateTime.UtcNow;
            var status = duration.Start <= createdAt ? PlannerStatus.InProgress : PlannerStatus.PendingStart;

            Name = name;
            Description = description;
            Duration = duration;
            PlannerId = plannerId;
            Status = status;
            Frequency = frequency?.ToString() ?? "*";
            TrackingType = trackingType;
            EqualType = equalType;
            AbstractGoalValue = abstractGoalValue;

            _items.Add(new GoalStatusItem(Status, "Goal initialized", DateTime.UtcNow));
        }

        #endregion

        #region Methods

        public void Update(
            string name, string description, Duration duration)
        {
            var isStartDateChanged = duration.Start.Date != Duration.Start.Date;
            var isEndDateChanged = duration.End.Date != Duration.End.Date;

            if ((isStartDateChanged || isEndDateChanged) && Status is PlannerStatus.Completed or PlannerStatus.Stopped)
            {
                throw new DomainException(
                    "Update goal duration is unavailable when planner is completed or stopped");
            }

            if (isStartDateChanged && Status != PlannerStatus.PendingStart)
            {
                throw new DomainException("Update goal start date is unavailable when planner is in progress");
            }

            if (duration.End.Date < Duration.End.Date && Status is not PlannerStatus.PendingStart)
            {
                throw new DomainException("Update goal end date is unavailable when goal is in progress");
            }

            Name = name;
            Description = description;
            Duration = duration;
        }

        public void ProgressGoal(string description = null, bool isManualSet = true)
        {
            if (Status is not (PlannerStatus.PendingStart or PlannerStatus.Postponed))
            {
                throw new DomainException(
                    $"Is not possible to set the goal status to {PlannerStatus.InProgress} from {Status}.");
            }

            if (!isManualSet && Status == PlannerStatus.Postponed)
            {
                return;
            }

            var status = Duration.Start.Date <= DateTime.UtcNow.Date ? PlannerStatus.InProgress : PlannerStatus.PendingStart;
            var item = new GoalStatusItem(
                status,
                description ?? "Goal started",
                DateTime.UtcNow
            );

            _items.Add(item);
            Status = PlannerStatus.InProgress;
        }

        public void PostponeGoal(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentNullException(nameof(reason), $"{nameof(reason)} can't be empty.");
            }

            if (Status is not (PlannerStatus.InProgress or PlannerStatus.PendingStart))
            {
                throw new DomainException(
                    $"Is not possible to set the goal status to {PlannerStatus.Postponed} from {Status}.");
            }

            var item = new GoalStatusItem(
                PlannerStatus.Postponed,
                reason,
                DateTime.UtcNow
            );

            _items.Add(item);
            Status = PlannerStatus.Postponed;
        }

        public void StopGoal(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentNullException(nameof(reason), $"{nameof(reason)} can't be empty.");
            }

            if (Status is PlannerStatus.Completed or PlannerStatus.Stopped)
            {
                throw new DomainException(
                    $"Is not possible to set the goal status to {PlannerStatus.Stopped} from {Status}.");
            }

            var item = new GoalStatusItem(
                PlannerStatus.Stopped,
                reason,
                DateTime.UtcNow
            );

            _items.Add(item);

            Status = PlannerStatus.Stopped;
        }

        public void CompleteGoal(string reason = null)
        {
            var item = new GoalStatusItem(
                PlannerStatus.Completed,
                reason ?? "The goal's end date is expired. Goal completed.",
                DateTime.UtcNow
            );

            _items.Add(item);

            Status = PlannerStatus.Completed;
        }

        public Report CreateReport(string description, DateTime date, decimal valueOfProgress)
        {
            if (!IsInProgress())
            {
                throw new DomainException($"Is not possible to add report, when the goal is {Status}");
            }

            if (!Duration.Contains(date))
            {
                throw new DomainException("Date of report does not compatible with goal duration");
            }

            return new Report(description, date, valueOfProgress, TrackingType, Id);
        }

        public void UpdateReport(Guid reportId, string description, DateTime date, decimal valueOfProgress)
        {
            if (!IsInProgress())
            {
                throw new DomainException($"Is not possible to remove report, when the goal is {Status}");
            }

            if (!Duration.Contains(date))
            {
                throw new DomainException("Date of report does not compatible with goal duration");
            }

            var report = _reports.FirstOrDefault(x => x.Id == reportId);

            if (report is null)
            {
                throw AggregateNotFoundException.For<Report>(reportId);
            }

            report.Update(description, date, valueOfProgress);
        }

        public Report GetReportForDeleting(Guid reportId)
        {
            if (!IsInProgress())
            {
                throw new DomainException($"Is not possible to remove report, when the goal is {Status}");
            }

            var report = _reports.FirstOrDefault(x => x.Id == reportId);

            if (report is null)
            {
                throw AggregateNotFoundException.For<Report>(reportId);
            }

            return report;
        }

        private bool IsInProgress() => Status == PlannerStatus.InProgress;

        #endregion
    }
}