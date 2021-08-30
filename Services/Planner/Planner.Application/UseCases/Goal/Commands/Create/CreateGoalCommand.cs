using MediatR;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using Planner.Domain.AggregatesModel.GoalAggregate.ValueObjects;
using Planner.Domain.ValueObjects;
using System;

namespace Planner.Application.UseCases.Goal.Commands.Create
{
    public class CreateGoalCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Duration Duration { get; set; }

        public Frequency Frequency { get; set; }

        public Guid PlannerId { get; set; }

        public decimal? AbstractGoalValue { get; set; }

        public TrackingType TrackingType { get; set; }

        public EqualType EqualType { get; set; }
    }
}