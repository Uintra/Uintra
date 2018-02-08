using System;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.Notification
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public Enum Type { get; set; }
        public IIntranetUser Notifier { get; set; }
        public dynamic Value { get; set; }
    }
}