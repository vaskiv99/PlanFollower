using BuildingBlocks.Domain.Events.Abstractions;
using System;

namespace BuildingBlocks.Domain.Entity.Abstractions
{
    public interface IEntity<out T>
    {
        T Id { get; }

        IEvent[] DequeueUncommittedEvents(bool clear = true);
    }

    public interface IEntity : IEntity<Guid>
    {
    }
}