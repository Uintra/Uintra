using System;
using uCommunity.Core.App_Plugins.Core.User;

namespace uCommunity.Core.App_Plugins.Core.Activity.Models
{
    public abstract class IntranetActivityModelBase
    {
        public Guid Id { get; set; }
        public IntranetActivityTypeEnum Type { get; set; }
        public string Title { get; set; }
        public int? UmbracoCreatorId { get; set; }
        public virtual IntranetUserBase Creator { get; set; }
        public bool CanEdit { get; set; }
    }
}