using System;
using Uintra20.Core.User.Models;
using Uintra20.Features.Permissions.Models;

namespace Uintra20.Core.Member.Abstractions
{
    public interface IIntranetMember
	{
        Guid Id { get; set; }
        string DisplayedName { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string LoginName { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Department { get; set; }
        string Photo { get; set; }
        int? PhotoId { get; set; }
        IntranetMemberGroup[] Groups { get; set; }
        IIntranetUser RelatedUser { get; set; }
        int UmbracoId { get; set; }
        bool Inactive { get; set; }
        bool IsSuperUser { get; set; }
    }
}
