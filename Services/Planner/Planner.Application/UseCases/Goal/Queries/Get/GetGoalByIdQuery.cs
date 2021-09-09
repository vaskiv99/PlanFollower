using MediatR;
using System;

namespace Planner.Application.UseCases.Goal.Queries.Get
{
    public class GetGoalByIdQuery : IRequest<ExtendedGoalView>
    {
        public Guid Id { get; set; }
    }
}