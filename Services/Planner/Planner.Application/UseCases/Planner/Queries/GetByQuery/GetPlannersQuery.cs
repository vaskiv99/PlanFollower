using BuildingBlocks.Common.Models;
using MediatR;
using Planner.Domain.Enum;
using System.Collections.Generic;

namespace Planner.Application.UseCases.Planner.Queries.GetByQuery
{
    public class GetPlannersQuery : IRequest<QueryResult<PlannerView>>
    {
        public PageQuery PageQuery { get; set; }

        public List<PlannerStatus> Statuses { get; set; }
    }
}