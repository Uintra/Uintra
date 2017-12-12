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

        public virtual IEnumerable<NotificationSettingsCategoryDto> GetAvailableCategories()
        {
            return GetBulletinSettings().ToEnumerableOfOne().Append(GetNewsSettings()).Append(GetEventSettings());
        }

        protected virtual IIntranetType[] CommentNotificationTypes => new[]
        {
            GetIntranetType(NotificationTypeEnum.CommentAdded),
            GetIntranetType(NotificationTypeEnum.CommentEdited),
            GetIntranetType(NotificationTypeEnum.CommentReplied)
        };

        protected virtual NotificationSettingsCategoryDto GetBulletinSettings()
        {
            var notificationTypes =
                CommentNotificationTypes.Append(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded));

            return new NotificationSettingsCategoryDto(GetIntranetType(IntranetActivityTypeEnum.Bulletins), notificationTypes);
        }

        protected virtual NotificationSettingsCategoryDto GetNewsSettings()
        {
            var notificationTypes =
                CommentNotificationTypes.Append(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded));

            return new NotificationSettingsCategoryDto(GetIntranetType(IntranetActivityTypeEnum.News), notificationTypes);
        }

        protected virtual NotificationSettingsCategoryDto GetEventSettings()
        {
            var eventNotificationTypes = new[]
            {
                GetIntranetType(NotificationTypeEnum.EventUpdated),
                GetIntranetType(NotificationTypeEnum.EventHided),
                GetIntranetType(NotificationTypeEnum.BeforeStart),
            };

            var notificationTypes =
                eventNotificationTypes
                .Concat(CommentNotificationTypes)
                .Append(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded));

            return new NotificationSettingsCategoryDto(GetIntranetType(IntranetActivityTypeEnum.Events), notificationTypes);
        }


        protected IIntranetType GetIntranetType(NotificationTypeEnum type) => _notificationTypeProvider.Get((int)type);
        protected IIntranetType GetIntranetType(IntranetActivityTypeEnum type) => _activityTypeProvider.Get((int)type);
    }
}