using System;

namespace uIntra.Core.User
{
    public interface IHaveOwner
    {
        Guid OwnerId { get; }
    }
}