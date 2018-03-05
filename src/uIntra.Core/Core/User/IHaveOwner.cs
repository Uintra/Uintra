using System;

namespace Uintra.Core.User
{
    public interface IHaveOwner
    {
        Guid OwnerId { get; }
    }
}