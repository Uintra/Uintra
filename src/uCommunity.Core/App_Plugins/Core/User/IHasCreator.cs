using System;
using uCommunity.Core.App_Plugins.Core.User;

namespace TeamDenmark.Intranet.App_Plugins.Core.User
{
    public interface IHasCreator
    {
        int? UmbracoCreatorId { get; set; }

        Guid CreatorId { get; set; }

        IntranetUserBase Creator { get; set; }
    }
}