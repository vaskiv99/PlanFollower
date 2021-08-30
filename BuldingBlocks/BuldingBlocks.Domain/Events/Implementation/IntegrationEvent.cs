using BuildingBlocks.Domain.Events.Abstractions;
using System;

namespace BuildingBlocks.Domain.Events.Implementation
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.UtcNow;
        }

        protected IntegrationEvent(Guid id, DateTime createDate)
        {
            this.Id = id;
            this.CreationDate = createDate;
        }

        public Guid Id { get; private set; }

        public DateTime CreationDate { get; private set; }
    }
}