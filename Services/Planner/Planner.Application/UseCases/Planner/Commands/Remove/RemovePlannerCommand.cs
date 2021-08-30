using MediatR;
using System;

namespace Planner.Application.UseCases.Planner.Commands.Remove
{
    public class RemovePlannerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}