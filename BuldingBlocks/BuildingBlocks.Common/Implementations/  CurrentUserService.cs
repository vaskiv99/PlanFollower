using BuildingBlocks.Common.Interfaces;
using System;

namespace BuildingBlocks.Common.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid GetUserId()
        {
            return Guid.Parse("f5c2f36c-ec31-4cfe-a4e5-d707fc00542e");
        }
    }
}