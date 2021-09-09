using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using System;

namespace Planner.Application.UseCases.Goal.Queries
{
    public record ReportView
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal ValueOfProgress { get; set; }

        public TrackingType TrackingType { get; set; }
    }
}