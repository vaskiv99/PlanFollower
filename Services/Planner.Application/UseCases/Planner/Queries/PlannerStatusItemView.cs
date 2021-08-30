using Planner.Domain.Enum;
using System;

namespace Planner.Application.UseCases.Planner.Queries
{
    public record PlannerStatusItemView
    {
        public Guid Id { get; set; }

        public PlannerStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}