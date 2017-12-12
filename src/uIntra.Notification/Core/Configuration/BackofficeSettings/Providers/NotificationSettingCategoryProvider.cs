using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Configuration
{
    public class NotificationSettingCategoryProvider : INotificationSettingCategoryProvider
    {
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public NotificationSettingCategoryProvider(IActivityTypeProvider activityTypeProvider, INotificationTypeProvider notificationTypeProvider)
        {
            _activityTypeProvider = activityTypeProvider;
            _notificationTypeProvider = notificationTypeProvider;
        }

        public IEnumerable<NotificationSettingsCategoryDto> GetAvailableCategories()
        {
            var commentNotificationTypes = new[] 
            {
                GetIntranetType(NotificationTypeEnum.CommentAdded),
                GetIntranetType(NotificationTypeEnum.CommentEdited),
                GetIntranetType(NotificationTypeEnum.CommentReplied) 
            };

            var bulletinSettings = new NotificationSettingsCategoryDto(
                GetIntranetType(IntranetActivityTypeEnum.Bulletins),
                commentNotificationTypes.Append(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded))
               );

            var newsSettings = new NotificationSettingsCategoryDto(
                GetIntranetType(IntranetActivityTypeEnum.News),
                commentNotificationTypes.Append(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded))
            );

            var eventNotificationTypes = new[]
            {
                GetIntranetType(NotificationTypeEnum.EventUpdated),
                GetIntranetType(NotificationTypeEnum.EventHided),
                GetIntranetType(NotificationTypeEnum.BeforeStart),
            };

            var eventSettings = new NotificationSettingsCategoryDto(
                GetIntranetType(IntranetActivityTypeEnum.Events),
                commentNotificationTypes.Append(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded)).Concat(eventNotificationTypes)
            );
            return bulletinSettings.ToEnumerableOfOne().Append(newsSettings).Append(eventSettings);
        }

        protected IIntranetType GetIntranetType(NotificationTypeEnum type) => _notificationTypeProvider.Get((int)type);
        protected IIntranetType GetIntranetType(IntranetActivityTypeEnum type) => _activityTypeProvider.Get((int)type);
    }
}