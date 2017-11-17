namespace uIntra.Notification.Configuration
{
    public interface IDefaultTemplateReader
    {
        string ReadTemplate(ActivityEventNotifierIdentity notificationType);
    }
}