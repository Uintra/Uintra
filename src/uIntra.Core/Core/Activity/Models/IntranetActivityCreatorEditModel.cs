using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Links;
using uIntra.Core.User;

namespace uIntra.Core.Activity
{
    public class IntranetActivityCreatorEditModel
    {
        public IIntranetUser Creator { get; set; }
        public IEnumerable<IntranetActivityCreatorViewModel> Users { get; set; } = Enumerable.Empty<IntranetActivityCreatorViewModel>();
        public bool CanEditCreator { get; set; }
        public string CreatorIdPropertyName { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}