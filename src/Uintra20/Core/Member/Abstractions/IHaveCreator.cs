using System;

namespace Uintra20.Core.Member.Abstractions
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }
        Guid CreatorId { get; }
    }
}
