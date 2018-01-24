using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Configuration
{
    public class NotificationSettingsCategoryDto
    {
        public IIntranetType ActivityType { get; }
        public IEnumerable<Enum> NotificationTypes { get; }

        public NotificationSettingsCategoryDto(IIntranetType activityType) : this(activityType, Enumerable.Empty<Enum>())
        {}

        public NotificationSettingsCategoryDto(IIntranetType activityType, IEnumerable<Enum> notificationTypes)
        {
            ActivityType = activityType;
            NotificationTypes = notificationTypes;
        }
    }
}