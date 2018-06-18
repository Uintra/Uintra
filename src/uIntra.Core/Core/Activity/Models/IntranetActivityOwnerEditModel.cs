using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Links;
using uIntra.Core.User;

namespace uIntra.Core.Activity
{
    public class IntranetActivityOwnerEditModel
    {
        public IIntranetUser Owner { get; set; }
        public IEnumerable<IIntranetUser> Users { get; set; } = Enumerable.Empty<IIntranetUser>();
        public bool CanEditOwner { get; set; }
        public string OwnerIdPropertyName { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}