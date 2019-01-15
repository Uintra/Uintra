using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Uintra.Core.Activity
{
    public class IntranetActivityOwnerEditModel
    {
        public UserViewModel Owner { get; set; }
        public IEnumerable<IIntranetUser> Users { get; set; } = Enumerable.Empty<IIntranetUser>();
        public bool CanEditOwner { get; set; }
        public string OwnerIdPropertyName { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}