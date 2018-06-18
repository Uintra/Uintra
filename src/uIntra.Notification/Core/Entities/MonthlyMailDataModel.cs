using System;
using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public class MonthlyMailDataModel : INotifierDataValue
    {
        public string Url { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public string ActivityList { get; set; }
    }
}
