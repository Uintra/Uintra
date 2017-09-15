using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.User;
using uIntra.Groups;

namespace Compent.uIntra.Core.Users
{
    public class IntranetUser : IGroupMember
    {
        public Guid Id { get; set; } 
        public int? UmbracoId { get; set; }
        public virtual string DisplayedName => $"{FirstName} {LastName}";
        public virtual string Photo { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public bool Inactive { get; set; }
        public IRole Role { get; set; }

        public IEnumerable<Guid> GroupIds { get; set; } = Enumerable.Empty<Guid>();
    }
}
