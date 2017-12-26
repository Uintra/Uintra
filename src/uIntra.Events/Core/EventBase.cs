using System;
using uIntra.Core.Activity;
using uIntra.Core.User;

namespace uIntra.Events
{
    public class EventBase : IntranetActivity, IHaveCreator, IHaveOwner
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PublishDate { get; set; }
        public bool CanSubscribe { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public int? UmbracoCreatorId { get; set; }
    }
}
