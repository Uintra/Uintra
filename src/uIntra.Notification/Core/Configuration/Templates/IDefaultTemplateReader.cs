using uIntra.Notification.Core.Models;

namespace uIntra.Notification.Configuration
{
    public interface IDefaultTemplateReader
    {
        string ReadTemplate(ActivityEventIdentity notificationType);
    }
}