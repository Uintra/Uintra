using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Configuration
{
    public class NotificationSettingsCategoryDto
    {
        public IIntranetType ActivityType { get; }
        public IEnumerable<IIntranetType> NotificationTypes { get; }

        public NotificationSettingsCategoryDto(IIntranetType activityType) : this(activityType, Enumerable.Empty<IIntranetType>())
        {}

        public NotificationSettingsCategoryDto(IIntranetType activityType, IEnumerable<IIntranetType> notificationTypes)
        {
            ActivityType = activityType;
            NotificationTypes = notificationTypes;
        }
    }
}