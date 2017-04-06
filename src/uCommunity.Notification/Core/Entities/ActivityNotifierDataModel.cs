using System;
using uCommunity.Core.Activity;

namespace uCommunity.Notification.Core.Entities
{
    public class ActivityNotifierDataModel : INotifierDataValue, IHaveNotifierId
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
        public string NotifierName { get; set; }
        public IntranetActivityTypeEnum ActivityType { get; set; }
    }
}
