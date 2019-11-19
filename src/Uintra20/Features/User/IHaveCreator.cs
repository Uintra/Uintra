using System;

namespace Uintra20.Features.User
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }
        Guid CreatorId { get; }
    }
}
