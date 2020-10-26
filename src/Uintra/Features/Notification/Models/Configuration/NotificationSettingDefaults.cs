using Uintra.Features.Notification.Models.NotifierTemplates;

namespace Uintra.Features.Notification.Models.Configuration
{
    public class NotificationSettingDefaults<T>
        where T : INotifierTemplate
    {
        public NotificationSettingDefaults(string label, T template)
        {
            Label = label;
            Template = template;
            Template = template;
        }

        public string Label { get; }
        public T Template { get; }
    }
}