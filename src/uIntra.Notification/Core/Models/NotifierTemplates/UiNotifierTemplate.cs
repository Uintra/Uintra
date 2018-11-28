namespace Uintra.Notification
{
    public class UiNotifierTemplate : INotifierTemplate
    {
        public string Message { get; set; }
        public bool IsDesktopNotificationEnabled { get; set; }
        public string DesktopMessage { get; set; }
        public string DesktopTitle { get; set; }
    }
}