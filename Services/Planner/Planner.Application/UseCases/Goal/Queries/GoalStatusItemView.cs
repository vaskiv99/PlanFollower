using Planner.Domain.Enum;
using System;

namespace Planner.Application.UseCases.Goal.Queries
{
    public record GoalStatusItemView
    {
        public Guid Id { get; set; }

        public PlannerStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}