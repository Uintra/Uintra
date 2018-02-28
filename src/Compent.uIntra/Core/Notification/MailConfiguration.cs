using System.Web;
using Uintra.Core;
using Uintra.Core.Extensions;

namespace Compent.Uintra.Core.Notification
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