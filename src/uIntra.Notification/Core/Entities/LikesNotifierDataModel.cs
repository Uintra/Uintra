using System;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public class LikesNotifierDataModel : INotifierDataValue, IHaveNotifierId
    {
        public IIntranetType NotificationType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
