using System;
using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.User
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
        Option<string> Photo { get; set; }
        Option<int> PhotoId { get; set; }
        IEnumerable<IntranetMemberGroup> Groups { get; set; }
        Option<IIntranetUser> RelatedUser { get; set; }
        int UmbracoId { get; set; }
        bool Inactive { get; set; }
        bool IsSuperUser { get; set; }
    }
}
