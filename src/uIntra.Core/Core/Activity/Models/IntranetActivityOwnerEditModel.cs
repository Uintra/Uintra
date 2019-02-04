using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Uintra.Core.Activity
{
    public class IntranetActivityOwnerEditModel
    {
        public MemberViewModel Owner { get; set; }
        public IEnumerable<IIntranetMember> Members { get; set; } = Enumerable.Empty<IIntranetMember>();
        public bool CanEditOwner { get; set; }
        public string OwnerIdPropertyName { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}