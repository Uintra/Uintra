using uIntra.Core;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Core.Notification
{
    public class MailConfiguration
    {
        public static string MailTemplateXpath => XPathHelper.GetXpath(
            DataFolder.ModelTypeAlias, 
            MailTemplatesFolder.ModelTypeAlias,
            MailTemplate.ModelTypeAlias);
    }
}