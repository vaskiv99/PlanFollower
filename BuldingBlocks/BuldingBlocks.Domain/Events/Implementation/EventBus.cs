using BuildingBlocks.Domain.Events.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.Events.Implementation
{
    public class EventBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IExternalEventBus _externalEventBus;

        public EventBus(IMediator mediator, IExternalEventBus externalEventBus)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _externalEventBus = externalEventBus;
        }

        public async Task PublishAsync(IEvent[] events, CancellationToken cancellationToken)
        {
            foreach (var @event in events)
            {
                if (@event is IDomainEvent)
                {
                    await _mediator.Publish(@event, cancellationToken);
                }

                if (@event is IntegrationEvent integrationEvent)
                {
                    _externalEventBus?.Publish(integrationEvent);
                }
            }
        }
    }
}