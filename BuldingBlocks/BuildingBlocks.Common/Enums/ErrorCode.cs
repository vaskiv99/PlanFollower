namespace BuildingBlocks.Common.Enums
{
    /// <summary>
    /// Represent, application error codes
    /// </summary>
    public enum ErrorCode
    {
        Unknown,
        ValidationFailed,
        DomainError,
        AggregateNotFound,
        InvalidData,
        InvalidOperation
    }
}