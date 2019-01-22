using System;

namespace Uintra.Core.User
{
    public interface IIntranetMember
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
