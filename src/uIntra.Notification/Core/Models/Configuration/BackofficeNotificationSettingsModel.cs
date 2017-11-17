namespace uIntra.Notification
{
    public class BackofficeNotificationSettingsModel<T>
        where T : INotifierTemplate
    {
        public BackofficeNotificationSettingsModel(string label, T template)
        {
            Label = label;
            Template = template;
        }

        public string Label { get; }
        public T Template { get; }
    }
}