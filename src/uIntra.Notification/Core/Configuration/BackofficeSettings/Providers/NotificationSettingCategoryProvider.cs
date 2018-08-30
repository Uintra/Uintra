using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra.Core.Activity;

namespace Uintra.Notification.Configuration
{
    /// <summary>
    /// Is responsible for defining activities with notification types that would be displayed in backoffice section
    /// </summary>
    public class NotificationSettingCategoryProvider : INotificationSettingCategoryProvider
    {
        public virtual IEnumerable<NotificationSettingsCategoryDto> GetAvailableCategories() =>
            GetBulletinSettings()
                .ToEnumerable()
                .Append(GetNewsSettings())
                .Append(GetEventSettings())
                .Append(GetCommunicationSettings())
                .Append(GetMemberSettings());

        public virtual NotificationSettingsCategoryDto GetCommunicationSettings() => //TODO: temporary for communication settings
            new NotificationSettingsCategoryDto(
                CommunicationTypeEnum.CommunicationSettings,
                new Enum[] { NotificationTypeEnum.CommentLikeAdded, NotificationTypeEnum.MonthlyMail, NotificationTypeEnum.UserMention });
                .Append(NotificationTypeEnum.CommentReplied)

        public virtual NotificationSettingsCategoryDto
            GetMemberSettings() => //TODO: temporary for communication settings
            new NotificationSettingsCategoryDto(CommunicationTypeEnum.Member, ((Enum)NotificationTypeEnum.Welcome).ToEnumerable());


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