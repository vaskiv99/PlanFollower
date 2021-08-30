using BuildingBlocks.Common.Enums;

namespace BuildingBlocks.Common.Exceptions
{
    public class DomainException : BaseException
    {
        public DomainException(string message, ErrorCode errorCode = ErrorCode.DomainError) : base(message, errorCode)
        {
            StatusCode = 400;
        }
    }
}