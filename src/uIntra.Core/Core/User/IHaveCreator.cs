using System;

namespace Uintra.Core.User
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }

        Guid CreatorId { get; }
    }
}