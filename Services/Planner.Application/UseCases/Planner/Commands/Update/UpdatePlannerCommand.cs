using MediatR;
using Planner.Domain.ValueObjects;
using System;
using System.Text.Json.Serialization;

namespace Planner.Application.UseCases.Planner.Commands.Update
{
    public class UpdatePlannerCommand : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Duration Duration { get; set; }
    }
}