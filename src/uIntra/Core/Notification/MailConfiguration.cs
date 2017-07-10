using Umbraco.Web.PublishedContentModels;

namespace uIntra.Core.Notification
{
    public class MailConfiguration
    {
        public static string MailTemplateXpath => XPathHelper.GetXpath(
            DataFolder.ModelTypeAlias, 
            MailTemplatesFolder.ModelTypeAlias,
            MailTemplate.ModelTypeAlias);
    }
}