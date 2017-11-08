using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Core.Services
{
    public interface INotificationSettingsService
    {
        (IntranetActivityTypeEnum activityType, NotificationTypeEnum[] notificationTypes)[] NotificationPolicies { get; }
    }
}
