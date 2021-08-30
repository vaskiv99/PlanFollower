using MediatR;
using Planner.Domain.Enum;
using System;
using System.Text.Json.Serialization;

namespace Planner.Application.UseCases.Planner.Commands.UpdateStatus
{
    public class UpdateStatusCommand : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public PlannerStatus Status { get; set; }

        public string Reason { get; set; }
    }
}