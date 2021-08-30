namespace BuildingBlocks.Domain.Events.Abstractions
{
    public interface IExternalEventBus
    {
        void Publish<T>(T @event)
            where T : IIntegrationEvent;

        void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}