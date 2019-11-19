namespace Uintra20.Features.Notification.Entities.Base
{
    public interface INotifierDataValue
    {
        string Url { get; set; }
        bool IsPinned { get; set; }
        bool IsPinActual { get; set; }
    }
}
