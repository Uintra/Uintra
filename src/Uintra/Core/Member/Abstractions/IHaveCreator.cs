using System;

namespace Uintra.Core.Member.Abstractions
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }
        Guid CreatorId { get; }
    }
}
