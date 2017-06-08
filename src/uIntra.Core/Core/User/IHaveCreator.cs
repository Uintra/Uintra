using System;

namespace uIntra.Core.User
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }

        Guid CreatorId { get; }
    }
}