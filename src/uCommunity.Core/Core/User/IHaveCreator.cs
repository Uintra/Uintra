using System;

namespace uCommunity.Core.User
{
    public interface IHaveCreator
    {
        int? UmbracoCreatorId { get; }

        Guid CreatorId { get; }

        IIntranetUser Creator { get; set; }
    }
}