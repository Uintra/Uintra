using uIntra.Core;
using uIntra.Notification.Configuration;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Core.Mails
{
    public class MailConfiguration : IMailConfiguration
    {
        public string MailTemplateXpath => XPathHelper.GetXpath(
            DataFolder.ModelTypeAlias, 
            MailTemplatesFolder.ModelTypeAlias,
            MailTemplate.ModelTypeAlias);
    }
}