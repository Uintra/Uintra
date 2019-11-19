using System;

namespace Uintra20.Core.Member
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }
        Guid CreatorId { get; }
    }
}
