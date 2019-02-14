using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.User;
using Uintra.Groups;

namespace Compent.Uintra.Core.Users
{
    public class IntranetMember : IGroupMember
    {
        public Guid Id { get; set; } 
        public int? UmbracoId { get; set; }
        public virtual string DisplayedName => $"{FirstName} {LastName}";
        public virtual string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public string Phone { get; set; }
        public int? PhotoId { get; set; }
        public string Department { get; set; }
        public bool Inactive { get; set; }
        public IRole Role { get; set; }
        public IIntranetUser RelatedUser { get; set; }
        public bool IsSuperUser { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; } = Enumerable.Empty<Guid>();
    }
}
