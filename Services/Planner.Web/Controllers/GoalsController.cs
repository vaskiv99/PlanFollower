using BuildingBlocks.Common.Models;
using BuildingBlocks.Web.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Application.UseCases.Goal.Commands.Create;
using Planner.Application.UseCases.Goal.Commands.Remove;
using Planner.Application.UseCases.Goal.Queries;
using Planner.Application.UseCases.Goal.Queries.GetByQuery;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Web.Controllers
{
    [ApiVersion("0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GoalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpPost]
        public async Task<Guid> CreateAsync([FromBody, Required] CreateGoalCommand command,
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryResult<GoalView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpGet]
        public async Task<QueryResult<GoalView>> GetByQueryAsync([FromQuery, Required] GetGoalsQuery query,
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpDelete("{id}")]
        public async Task<EmptyResponse> RemoveAsync([FromRoute, Required] Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveGoalCommand { Id = id }, cancellationToken);

            return EmptyResponse.Instance;
        }
    }
}