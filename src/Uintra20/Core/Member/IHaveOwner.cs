using System;

namespace Uintra20.Core.Member
{
    public interface IHaveOwner
    {
        Guid OwnerId { get; }
    }
}
