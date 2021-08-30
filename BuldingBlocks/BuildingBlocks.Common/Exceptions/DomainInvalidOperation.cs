using BuildingBlocks.Common.Enums;
using System;

namespace BuildingBlocks.Common.Exceptions
{
    public class DomainInvalidOperation : BaseException
    {
        public DomainInvalidOperation(string message) : base(message, ErrorCode.InvalidOperation)
        {
            StatusCode = 400;
        }

        private DomainInvalidOperation(string typeName, Guid id)
            : base($"Invalid operation for {typeName} with id '{id}'", ErrorCode.InvalidOperation)
        {
            StatusCode = 400;
        }

        public static DomainInvalidOperation For<T>(Guid id) => new(typeof(T).Name, id);
    }
}