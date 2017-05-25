using System;
using uIntra.Core.Activity;

namespace uCommunity.Notification.Core.Entities
{
    public class LikesNotifierDataModel : INotifierDataValue, IHaveNotifierId
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
    }
}
