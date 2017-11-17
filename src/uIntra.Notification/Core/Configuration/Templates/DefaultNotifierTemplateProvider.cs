using uIntra.Core.Activity;
using uIntra.Notification.Core.Models;

namespace uIntra.Notification.Configuration
{
    public static class DefaultTemplatesConstants
    {
        public const string RootFolderName = "DefaultTemplates";
    }


    public class DefaultNotifierTemplateProvider : 
        IDefaultNotifierTemplateProvider<EmailNotifierTemplate>,
        IDefaultNotifierTemplateProvider<UiNotifierTemplate>
    {
        public EmailNotifierTemplate GetTemplate(ActivityEventIdentity notificationType)
        {
            throw new System.NotImplementedException();
        }

        UiNotifierTemplate IDefaultNotifierTemplateProvider<UiNotifierTemplate>.GetTemplate(ActivityEventIdentity notificationType)
        {
            throw new System.NotImplementedException();
        }
    }
}