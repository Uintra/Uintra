using System.Web;
using Uintra.Infrastructure.Extensions;
using Uintra.Infrastructure.Providers;

namespace Uintra.Features.Notification
{
    public static class MailConfiguration
    {
        static MailConfiguration()
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();

            MailTemplateXpath = string.Empty; //XPathHelper.GetXpath(//TODO:Research when mail service is ready
            //docTypeAliasProvider.GetDataFolder(),
            //docTypeAliasProvider.GetMailTemplateFolder(),
            //docTypeAliasProvider.GetMailTemplate());
        }
        public static string MailTemplateXpath { get; set; }
    }
}