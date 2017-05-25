using System;
using uIntra.Core.Activity;
using uIntra.Notification.Core.Entities.Base;

namespace uIntra.Notification.Core.Entities
{
    public class ActivityReminderDataModel : INotifierDataValue
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime StartDate { get; set; }
    }
}
