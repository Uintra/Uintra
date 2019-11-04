using System;

namespace Uintra20.Core.User
{
    public interface IHaveOwner
    {
        Guid OwnerId { get; }
    }
}
