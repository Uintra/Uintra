using System;

namespace uCommunity.Core.User
{
    public interface IIntranetUser
    {
        Guid Id { get; set; }
        int? UmbracoId { get; set; }
        string DisplayedName { get; set; }
        string Photo { get; set; }
    }
}
