using System.Web;
using Uintra20.Core.Extensions;

namespace Uintra20.Core.Notification
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