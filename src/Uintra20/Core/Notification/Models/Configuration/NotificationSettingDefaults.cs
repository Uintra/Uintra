namespace Uintra20.Core.Notification
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