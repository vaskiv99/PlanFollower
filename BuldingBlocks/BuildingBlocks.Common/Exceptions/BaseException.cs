using BuildingBlocks.Common.Enums;
using System;

namespace BuildingBlocks.Common.Exceptions
{
    public class BaseException : Exception
    {
        public object Errors { get; protected set; }

        public ErrorCode ErrorCode { get; protected set; }

        public int StatusCode { get; protected set; } = 400;

        public BaseException() { }

        public BaseException(string message, ErrorCode errorCode = ErrorCode.Unknown)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}