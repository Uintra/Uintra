using System;
using uIntra.Core.Activity;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public class ActivityReminderDataModel : INotifierDataValue
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime StartDate { get; set; }
    }
}
