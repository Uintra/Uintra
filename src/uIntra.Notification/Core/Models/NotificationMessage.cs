using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public interface INotificationMessage
    {

    }

    public class UiNotificationMessage : INotificationMessage
    {
        public IIntranetType NotificationType { get; set; }
        public Guid ReceiverId { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
    }
}
