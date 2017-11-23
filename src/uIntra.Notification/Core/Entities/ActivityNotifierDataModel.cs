using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public class ActivityNotifierDataModel : INotifierDataValue, IHaveNotifierId
    {
        public IIntranetType NotificationType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
        public Dictionary<string, string> Tokens { get; set; }
    }
}
