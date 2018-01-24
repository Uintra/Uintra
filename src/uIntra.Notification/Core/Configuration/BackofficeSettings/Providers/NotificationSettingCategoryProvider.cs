using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Configuration
{
    /// <summary>
    /// Is responsible for defining activities with notification types that would be displayed in backoffice section
    /// </summary>
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
            return GetBulletinSettings().ToEnumerable().Append(GetNewsSettings()).Append(GetEventSettings());
        }

        protected virtual Enum[] CommentNotificationTypes => new Enum[]
        {
            NotificationTypeEnum.CommentAdded,
            NotificationTypeEnum.CommentEdited,
            NotificationTypeEnum.CommentReplied
        };

        protected virtual NotificationSettingsCategoryDto GetBulletinSettings()
        {
            var notificationTypes =
                CommentNotificationTypes.Append(NotificationTypeEnum.ActivityLikeAdded);

            return new NotificationSettingsCategoryDto(GetIntranetType(IntranetActivityTypeEnum.Bulletins), notificationTypes);
        }

        protected virtual NotificationSettingsCategoryDto GetNewsSettings()
        {
            var notificationTypes =
                CommentNotificationTypes.Append(NotificationTypeEnum.ActivityLikeAdded);

            return new NotificationSettingsCategoryDto(GetIntranetType(IntranetActivityTypeEnum.News), notificationTypes);
        }

        protected virtual NotificationSettingsCategoryDto GetEventSettings()
        {
            var eventNotificationTypes = new Enum[]
            {
                NotificationTypeEnum.EventUpdated,
                NotificationTypeEnum.EventHided,
                NotificationTypeEnum.BeforeStart,
            };

            var notificationTypes =
                eventNotificationTypes
                .Concat(CommentNotificationTypes)
                .Append(NotificationTypeEnum.ActivityLikeAdded);

            return new NotificationSettingsCategoryDto(GetIntranetType(IntranetActivityTypeEnum.Events), notificationTypes);
        }


        protected IIntranetType GetIntranetType(IntranetActivityTypeEnum type) => _activityTypeProvider.Get((int)type);
    }
}