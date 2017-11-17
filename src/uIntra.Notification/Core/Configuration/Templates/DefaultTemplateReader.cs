using System.Reflection;
using uIntra.Core.Activity;
using uIntra.Core.Utils;
using uIntra.Notification.Core.Models;

namespace uIntra.Notification.Configuration
{
    public class DefaultTemplateReader : IDefaultTemplateReader
    {
        private const string RootFolderName = "DefaultTemplates";

        private readonly IEmbeddedResourceService _embeddedResourceService;

        public DefaultTemplateReader(IEmbeddedResourceService embeddedResourceService)
        {
            _embeddedResourceService = embeddedResourceService;
        }

        public string ReadTemplate(ActivityEventIdentity notificationType)
        {
            (string resourceName, Assembly assembly) = GetEmbeddedResource(notificationType);
            return _embeddedResourceService.ReadResourceContent(resourceName, assembly);
        }

        private (string resourceName, Assembly assembly) GetEmbeddedResource(ActivityEventIdentity notificationType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = GetEmbeddedResourceName(notificationType, assembly);
            return (resourceName, assembly);
        }

        private string GetEmbeddedResourceName(ActivityEventIdentity notificationType, Assembly assembly) => 
            $"{GetRootFolder(assembly)}.{GetEmbeddedResourceName(notificationType)}";

        private string GetEmbeddedResourceName(ActivityEventIdentity notificationType) => 
            $"{notificationType.ActivityType.ToString()}.{notificationType.ActivityType.ToString()}";

        private string GetRootFolder(Assembly assembly) => 
            $"{assembly.GetName().Name}.{RootFolderName}";
    }
}