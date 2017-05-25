using System;
using uIntra.Core.Activity;
using uIntra.Notification.Core.Entities.Base;

namespace uIntra.Notification.Core.Entities
{
    public class LikesNotifierDataModel : INotifierDataValue, IHaveNotifierId
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
    }
}
