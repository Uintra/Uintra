using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public IIntranetType Type { get; set; }
        public NotifierViewModel Notifier { get; set; }
        public dynamic Value { get; set; }
    }
}