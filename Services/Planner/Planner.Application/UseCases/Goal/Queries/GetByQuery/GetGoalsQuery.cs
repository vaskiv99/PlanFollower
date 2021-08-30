using BuildingBlocks.Common.Models;
using MediatR;
using System;

namespace Planner.Application.UseCases.Goal.Queries.GetByQuery
{
    public class GetGoalsQuery : IRequest<QueryResult<GoalView>>
    {
        public Guid PlannerId { get; set; }

        public PageQuery PageQuery { get; set; }
    }
}