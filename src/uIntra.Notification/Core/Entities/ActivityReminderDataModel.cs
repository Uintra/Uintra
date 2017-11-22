using System;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public class ActivityReminderDataModel : INotifierDataValue
    {
        public IIntranetType NotificationType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime StartDate { get; set; }
    }
}
