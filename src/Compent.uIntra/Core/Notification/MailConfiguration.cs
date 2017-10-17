using System.Web;
using uIntra.Core;
using uIntra.Core.Extensions;

namespace Compent.uIntra.Core.Notification
{
    public static class MailConfiguration
    {
        static MailConfiguration()
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();

            MailTemplateXpath = XPathHelper.GetXpath(
            docTypeAliasProvider.GetDataFolder(),
            docTypeAliasProvider.GetMailTemplateFolder(),
            docTypeAliasProvider.GetMailTemplate());
        }
        public static string MailTemplateXpath { get; set; }
    }
    
}