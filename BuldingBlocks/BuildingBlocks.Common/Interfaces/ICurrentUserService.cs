using System;

namespace BuildingBlocks.Common.Interfaces
{
    /// <summary>
    /// Service which provide information about current authentication user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Get user Id.
        /// </summary>
        /// <returns>User Id</returns>
        Guid GetUserId();
    }
}