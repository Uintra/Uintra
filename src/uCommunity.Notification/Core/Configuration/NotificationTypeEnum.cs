using ServiceStack.DataAnnotations;

namespace uCommunity.Notification.Core.Configuration
{
    [EnumAsInt]
    public enum NotificationTypeEnum
    {
        Event = 1,
        EventUpdated,
        EventHided,
        BeforeStart,
        News,
        Idea,        
        LikeAdded,
        CommentAdded,
        CommentEdited,
        CommentReplyed
    }
}