using System.Reflection;
using uIntra.Core.Utils;

namespace uIntra.Notification.Configuration
{
    public class DefaultTemplateReader : IDefaultTemplateReader
    {
        private const string RootFolderName = "BackofficeNotificationSettings";

        private readonly IEmbeddedResourceService _embeddedResourceService;

        public DefaultTemplateReader(IEmbeddedResourceService embeddedResourceService)
        {
            _embeddedResourceService = embeddedResourceService;
        }

        public string ReadTemplate(ActivityEventNotifierIdentity notificationType)
        {
            (string resourceName, Assembly assembly) = GetEmbeddedResource(notificationType);
            return _embeddedResourceService.ReadResourceContent(resourceName, assembly);
        }

        private (string resourceName, Assembly assembly) GetEmbeddedResource(ActivityEventNotifierIdentity notificationType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = GetEmbeddedResourceName(notificationType, assembly);
            return (resourceName, assembly);
        }

        private string GetEmbeddedResourceName(ActivityEventNotifierIdentity notificationType, Assembly assembly) => 
            $"{GetRootFolder(assembly)}.{GetEmbeddedResourceFileName(notificationType)}";

        private string GetEmbeddedResourceFileName(ActivityEventNotifierIdentity type) => 
            $"{type.NotifierType.ToString()}.{type.Event.ActivityType.ToString()}.{type.Event.NotificationType.ToString()}.json";

        private string GetRootFolder(Assembly assembly) => 
            $"{assembly.GetName().Name}.{RootFolderName}";
    }
}