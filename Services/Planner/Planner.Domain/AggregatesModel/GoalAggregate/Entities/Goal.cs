using BuildingBlocks.Domain.Aggregate;
using BuildingBlocks.Domain.Entity.Implementation;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using Planner.Domain.AggregatesModel.GoalAggregate.ValueObjects;
using Planner.Domain.Enum;
using Planner.Domain.ValueObjects;
using System;

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
        /// For build this value <see cref="GoalAggregate.ValueObjects.Frequency"/>
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
            Frequency = frequency.ToString();
            TrackingType = trackingType;
            EqualType = equalType;
            AbstractGoalValue = abstractGoalValue;
        }

        #endregion
    }
}