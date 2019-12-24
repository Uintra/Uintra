using System;
using System.Collections.Generic;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public class NotificationTypeProvider : EnumTypeProviderBase, INotificationTypeProvider
    {
        public NotificationTypeProvider(params Type[] enums) : base(enums)
        {

        }

        //public IEnumerable<Enum> PopupNotificationTypes() => 
        //    List<Enum>(NotificationTypeEnum.Welcome);

        //public IEnumerable<Enum> UiNotificationTypes() => 
        //    All.Except(List(base[NotificationTypeEnum.Welcome.ToInt()]));
        public IEnumerable<Enum> PopupNotificationTypes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enum> UiNotificationTypes()
        {
            throw new NotImplementedException();
        }
    }
}
