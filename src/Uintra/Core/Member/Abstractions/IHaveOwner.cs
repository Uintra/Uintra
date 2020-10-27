using System;

namespace Uintra.Core.Member.Abstractions
{
    public interface IHaveOwner
    {
        Guid OwnerId { get; }
    }
}
