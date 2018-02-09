using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;

namespace Uintra.Notification.Configuration
{
    /// <summary>
    /// Is responsible for defining activities with notification types that would be displayed in backoffice section
    /// </summary>
    public class NotificationSettingCategoryProvider : INotificationSettingCategoryProvider
    {
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

            return new NotificationSettingsCategoryDto(IntranetActivityTypeEnum.Bulletins, notificationTypes);
        }

        protected virtual NotificationSettingsCategoryDto GetNewsSettings()
        {
            var notificationTypes = CommentNotificationTypes.Append(NotificationTypeEnum.ActivityLikeAdded);

            return new NotificationSettingsCategoryDto(IntranetActivityTypeEnum.News, notificationTypes);
        }

        protected virtual NotificationSettingsCategoryDto GetEventSettings()
        {
            var eventNotificationTypes = new Enum[]
            {
                NotificationTypeEnum.EventUpdated,
                NotificationTypeEnum.EventHided,
                NotificationTypeEnum.BeforeStart
            };

            var notificationTypes = eventNotificationTypes
                .Concat(CommentNotificationTypes)
                .Append(NotificationTypeEnum.ActivityLikeAdded);

            return new NotificationSettingsCategoryDto(IntranetActivityTypeEnum.Events, notificationTypes);
        }
    }
}