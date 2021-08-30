using MediatR;
using System;

namespace Planner.Application.UseCases.Planner.Queries.Get
{
    public class GetPlannerQuery : IRequest<PlannerView>
    {
        public Guid Id { get; set; }
    }
}