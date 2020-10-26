using Uintra20.Features.Links.Models;
using Uintra20.Features.Notification.Entities.Base;

namespace Uintra20.Features.Notification.Entities
{
    public class MonthlyMailDataModel : INotifierDataValue
    {
        public UintraLinkModel Url { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public string ActivityList { get; set; }
    }
}