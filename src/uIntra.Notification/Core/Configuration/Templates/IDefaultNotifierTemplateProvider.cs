using uIntra.Core.Activity;
using uIntra.Notification.Core.Models;

namespace uIntra.Notification.Configuration
{
    public interface IDefaultNotifierTemplateProvider<out T>
        where T : INotifierTemplate
    {
        T GetTemplate(ActivityEventIdentity notificationType);
    }
}