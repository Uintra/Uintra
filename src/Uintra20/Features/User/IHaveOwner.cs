using System;

namespace Uintra20.Features.User
{
    public interface IHaveOwner
    {
        Guid OwnerId { get; }
    }
}
