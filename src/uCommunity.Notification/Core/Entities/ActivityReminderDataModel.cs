using System;
using uCommunity.Core.Activity;

namespace uCommunity.Notification.Core.Entities
{
    public class ActivityReminderDataModel : INotifierDataValue
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public string Url { get; set; }

        public IntranetActivityTypeEnum ActivityType { get; set; }
    }
}
