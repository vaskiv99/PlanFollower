using MediatR;
using System;
using System.Text.Json.Serialization;

namespace Planner.Application.UseCases.Goal.Commands.AddReport
{
    public class AddReportCommand : IRequest
    {
        [JsonIgnore]
        public Guid GoalId { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal ValueOfProgress { get; set; }
    }
}