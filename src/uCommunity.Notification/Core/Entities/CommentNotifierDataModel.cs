using System;
using uCommunity.Core.Activity;

namespace uCommunity.Notification.Core.Entities
{
    public class CommentNotifierDataModel: INotifierDataValue, IHaveNotifierId
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
        public Guid CommentId { get; set; }
    }
}
