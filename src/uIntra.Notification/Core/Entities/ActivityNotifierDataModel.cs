using System;
using uIntra.Core.Activity;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public class ActivityNotifierDataModel : INotifierDataValue, IHaveNotifierId
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
    }
}
