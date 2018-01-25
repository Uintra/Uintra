using System.IO;
using System.Reflection;
using uIntra.Core.Utils;

namespace uIntra.Notification.Configuration
{
    public class BackofficeSettingsReader : IBackofficeSettingsReader
    {
        private const string RootFolderName = "BackofficeNotificationSettings";

        private readonly IEmbeddedResourceService _embeddedResourceService;

        public BackofficeSettingsReader(IEmbeddedResourceService embeddedResourceService)
        {
            _embeddedResourceService = embeddedResourceService;
        }

        public string ReadSettings(ActivityEventNotifierIdentity notificationType)
        {
            (string resourceName, Assembly assembly) = GetEmbeddedResource(notificationType);
            try
            {
                return _embeddedResourceService.ReadResourceContent(resourceName, assembly);
            }
            catch (FileNotFoundException)
            {
                string description =
                    "Embedded resource with config for notification(" +
                    $"{notificationType.NotifierType}, {notificationType.Event.ActivityType}, {notificationType.Event.NotificationType}" +
                    $") was not found at path {resourceName}.";
                throw new FileNotFoundException(description);
            }
        }

        protected virtual (string resourceName, Assembly assembly) GetEmbeddedResource(ActivityEventNotifierIdentity notificationType)
        {
            Assembly assembly = GetResourceAssembly(notificationType);
            string resourceName = GetEmbeddedResourceName(notificationType, assembly);
            return (resourceName, assembly);
        }

        protected virtual Assembly GetResourceAssembly(ActivityEventNotifierIdentity notificationType) => Assembly.GetExecutingAssembly();

        protected virtual string GetEmbeddedResourceName(ActivityEventNotifierIdentity notificationType, Assembly assembly) => 
            $"{GetRootFolder(assembly)}.{GetEmbeddedResourceFileName(notificationType)}";

        protected virtual string GetEmbeddedResourceFileName(ActivityEventNotifierIdentity type) => 
            $"{type.NotifierType}.{type.Event.ActivityType.ToString()}.{type.Event.NotificationType.ToString()}.json";

        protected virtual string GetRootFolder(Assembly assembly) => 
            $"{assembly.GetName().Name}.{RootFolderName}";
    }
}