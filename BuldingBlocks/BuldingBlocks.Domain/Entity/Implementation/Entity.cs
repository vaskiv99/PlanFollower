using BuildingBlocks.Domain.Entity.Abstractions;
using BuildingBlocks.Domain.Events.Abstractions;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Domain.Entity.Implementation
{
    public abstract class Entity<T> : IEntity<T> where T : notnull
    {
        public T Id { get; protected set; } = default!;

        [NonSerialized]
        private readonly Queue<IEvent> _uncommittedEvents = new();

        public IEvent[] DequeueUncommittedEvents(bool clear = true)
        {
            var dequeuedEvents = _uncommittedEvents.ToArray();

            if (clear)
                _uncommittedEvents.Clear();

            return dequeuedEvents;
        }

        protected void Enqueue(IEvent @event)
        {
            _uncommittedEvents.Enqueue(@event);
        }
    }

    public abstract class Entity : Entity<Guid>, IEntity
    {
    }
}