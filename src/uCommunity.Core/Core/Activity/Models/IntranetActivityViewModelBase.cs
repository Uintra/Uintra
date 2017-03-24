using System;
using uCommunity.Core.User;

namespace uCommunity.Core.Activity.Models
{
    public abstract class IntranetActivityViewModelBase : IHaveCreator
    {
        public Guid Id { get; set; }
        public IntranetActivityTypeEnum Type { get; set; }
        public string Title { get; set; }
        public bool CanEdit { get; set; }
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public IIntranetUser Creator { get; set; }
    }
}