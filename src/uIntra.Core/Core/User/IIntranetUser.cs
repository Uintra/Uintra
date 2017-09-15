using System;

namespace uIntra.Core.User
{
    public interface IIntranetUser
    {
        Guid Id { get; set; }
        int? UmbracoId { get; set; }
        string DisplayedName { get; }
        string Photo { get; set; }
        IRole Role { get; set; }
        string Email { get; set; }
        string LoginName { get; set; }
        bool Inactive { get; set; }
    }
}
