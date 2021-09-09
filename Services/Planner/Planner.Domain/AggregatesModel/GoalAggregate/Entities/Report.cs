using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Domain.Entity.Implementation;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using System;

namespace Planner.Domain.AggregatesModel.GoalAggregate.Entities
{
    public class Report : Entity
    {
        #region Properties

        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public decimal ValueOfProgress { get; private set; }

        public TrackingType TrackingType { get; private set; }

        public Guid GoalId { get; private set; }

        #endregion

        #region Constructors

        public Report() { }

        public Report(string description, DateTime date, decimal valueOfProgress, TrackingType type, Guid goalId)
        {
            if (date > DateTime.UtcNow)
            {
                throw new DomainException("Create a report for future dates is unavailable");
            }

            Id = Guid.NewGuid();
            Description = description;
            Date = date;
            ValueOfProgress = valueOfProgress;
            TrackingType = type;
            GoalId = goalId;
        }

        #endregion

        #region Methods

        public void Update(string description, DateTime date, decimal valueOfProgress)
        {
            Description = description;
            Date = date;
            ValueOfProgress = valueOfProgress;
        }

        #endregion
    }
}