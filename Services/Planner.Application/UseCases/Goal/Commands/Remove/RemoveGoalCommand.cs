using MediatR;
using System;

namespace Planner.Application.UseCases.Goal.Commands.Remove
{
    public class RemoveGoalCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}