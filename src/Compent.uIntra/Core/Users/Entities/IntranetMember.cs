using System;
using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.Permissions.Models;
using Uintra.Core.User;
using Uintra.Groups;

namespace Compent.Uintra.Core.Users
{
    public class IntranetMember : IGroupMember
    {
        public Guid Id { get; set; }
        public virtual string DisplayedName => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public Option<string> Photo { get; set; }
        public Option<int> PhotoId { get; set; }
        public IntranetMemberGroup Group { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
        public Option<IIntranetUser> RelatedUser { get; set; }
        public int UmbracoId { get; set; }
        public bool Inactive { get; set; }   
        public bool IsSuperUser { get; set; }
    }
}
