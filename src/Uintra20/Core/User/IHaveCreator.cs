using System;

namespace Uintra20.Core.User
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }
        Guid CreatorId { get; }
    }
}
