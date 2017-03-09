using System;
using TeamDenmark.Intranet.Core.Users;

namespace TeamDenmark.Intranet.App_Plugins.Core.User
{
    public interface IHasCreator
    {
        int? UmbracoCreatorId { get; set; }

        Guid CreatorId { get; set; }

        IntranetUser Creator { get; set; }
    }
}