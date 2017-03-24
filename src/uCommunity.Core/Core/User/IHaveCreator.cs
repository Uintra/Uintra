using System;

namespace uCommunity.Core.User
{
    public interface IHaveCreator
    {
        //TODO: Remove ids

        int? UmbracoCreatorId { get; }

        Guid CreatorId { get; }

        IIntranetUser Creator { get; set; }
    }
}