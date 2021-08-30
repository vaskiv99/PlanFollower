using Planner.Domain.Enum;
using Planner.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Planner.Application.UseCases.Planner.Queries
{
    public record PlannerView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Duration Duration { get; set; }

        public Guid UserId { get; set; }

        public PlannerStatus CurrentStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<PlannerStatusItemView> Items { get; set; }
    }
}