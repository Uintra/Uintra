using ServiceStack.DataAnnotations;

namespace uCommunity.Notification
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