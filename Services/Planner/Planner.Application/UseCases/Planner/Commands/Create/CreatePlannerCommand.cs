using MediatR;
using Planner.Domain.ValueObjects;
using System;

namespace Planner.Application.UseCases.Planner.Commands.Create
{
    public class CreatePlannerCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Duration Duration { get; set; }
    }
}