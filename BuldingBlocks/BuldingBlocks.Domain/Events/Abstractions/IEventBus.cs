using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.Events.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IEvent[] events, CancellationToken cancellationToken);
    }
}