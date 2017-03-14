using System;

namespace uCommunity.Core.User
{
    public interface IHasCreator<T>
        where T : IntranetUserBase
    {
        int? UmbracoCreatorId { get; }

        Guid CreatorId { get; }

        T Creator { get; set; }
    }
}