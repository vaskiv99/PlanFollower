using System.Threading.Tasks;

namespace BuildingBlocks.Domain.Events.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}