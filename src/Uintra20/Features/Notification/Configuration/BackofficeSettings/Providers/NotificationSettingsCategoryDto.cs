using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public class NotificationSettingsCategoryDto
    {
        public Enum ActivityType { get; }
        public IEnumerable<Enum> NotificationTypes { get; }

        public NotificationSettingsCategoryDto(Enum activityType) : this(activityType, Enumerable.Empty<Enum>())
        {}

        public NotificationSettingsCategoryDto(Enum activityType, IEnumerable<Enum> notificationTypes)
        {
            ActivityType = activityType;
            NotificationTypes = notificationTypes;
        }
    }
}