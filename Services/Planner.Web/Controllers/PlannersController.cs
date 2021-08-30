using BuildingBlocks.Web.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Application.UseCases.Planner.Commands.Create;
using Planner.Application.UseCases.Planner.Commands.Remove;
using Planner.Application.UseCases.Planner.Commands.Update;
using Planner.Application.UseCases.Planner.Commands.UpdateStatus;
using Planner.Application.UseCases.Planner.Queries;
using Planner.Application.UseCases.Planner.Queries.Get;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Web.Controllers
{
    [ApiVersion("0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PlannersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlannersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpPost]
        public async Task<Guid> CreateAsync([FromBody, Required] CreatePlannerCommand command,
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlannerView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpGet("{id}")]
        public async Task<PlannerView> GetAsync([FromRoute, Required] Guid id, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetPlannerQuery { Id = id }, cancellationToken);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpDelete("{id}")]
        public async Task<EmptyResponse> RemoveAsync([FromRoute, Required] Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemovePlannerCommand { Id = id }, cancellationToken);

            return EmptyResponse.Instance;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpPatch("{id}/status")]
        public async Task<EmptyResponse> UpdateStatusAsync(
            [FromRoute, Required] Guid id,
            [FromBody, Required] UpdateStatusCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            await _mediator.Send(command, cancellationToken);

            return EmptyResponse.Instance;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [HttpPut("{id}")]
        public async Task<EmptyResponse> UpdateAsync([FromRoute, Required] Guid id,
            [FromBody, Required] UpdatePlannerCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;

            await _mediator.Send(command, cancellationToken);

            return EmptyResponse.Instance;
        }
    }
}