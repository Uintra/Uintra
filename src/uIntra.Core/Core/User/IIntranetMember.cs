using System;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.User
{
    public interface IIntranetMember
    {
        Guid Id { get; set; }        
        string DisplayedName { get; }
        string Photo { get; set; }
        int? PhotoId { get; set; }
        IntranetMemberGroup Group { get; set; }
        string Email { get; set; }
        string LoginName { get; set; }
        bool Inactive { get; set; }
        IIntranetUser RelatedUser { get; set; }
        bool IsSuperUser { get; set; }
    }
}
