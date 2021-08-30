using BuildingBlocks.Common.Enums;
using System;

namespace BuildingBlocks.Common.Exceptions
{
    public class AggregateNotFoundException : DomainException
    {
        private AggregateNotFoundException(string typeName, Guid id)
            : base($"{typeName} with id '{id}' was not found", ErrorCode.AggregateNotFound)
        {
            StatusCode = 400;
        }

        public static AggregateNotFoundException For<T>(Guid id) => new(typeof(T).Name, id);
    }
}