using BuildingBlocks.Domain.Aggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.Repositories.Abstractions
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<T> Find(Guid id, CancellationToken cancellationToken);

        Task Add(T aggregate, CancellationToken cancellationToken);

        Task Update(T aggregate, CancellationToken cancellationToken);

        Task Delete(T aggregate, CancellationToken cancellationToken);
    }
}