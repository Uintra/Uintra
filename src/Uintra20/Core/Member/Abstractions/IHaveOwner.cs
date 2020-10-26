using System;

namespace Uintra20.Core.Member.Abstractions
{
    public interface IHaveOwner
    {
        Guid OwnerId { get; }
    }
}
